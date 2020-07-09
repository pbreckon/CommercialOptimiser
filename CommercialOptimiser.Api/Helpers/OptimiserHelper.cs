using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommercialOptimiser.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CommercialOptimiser.Api.Helpers
{
    public interface IOptimiserHelper
    {
        #region Public Methods

        List<BreakCommercials> GetOptimalBreakCommercials(
            List<Break> breaks,
            List<Commercial> commercials);

        #endregion
    }

    public class OptimiserHelper : IOptimiserHelper
    {
        #region Public Methods

        public List<BreakCommercials> GetOptimalBreakCommercials(
            List<Break> breaks,
            List<Commercial> commercials)
        {
            //Order break demographics by rating, and fulfill the highest 
            //rating downwards, as long we we don't break the rules:
            //1) No commercial types marked as invalid ("InvalidCommercialTypes") in 
            //a Break can be added to that Break
            //2) Commercials of the same type cannot be next to each other within a break

            var totalCommercialSpace = breaks.Sum(aBreak => aBreak.Capacity);
            if (totalCommercialSpace > commercials.Count())
                //Not enough commercials to fulfill the break capacities, 
                throw new ArgumentException(
                    "Number of commercials supplied is not sufficient to fulfill the break capacity");

            var orderedBreakDemographics = new List<OrderedBreakDemographic>();
            var allBreakCommercials = new List<BreakCommercials>();

            foreach (var aBreak in breaks)
            {
                orderedBreakDemographics.AddRange(
                    aBreak.BreakDemographics.Select(
                        bd =>
                            new OrderedBreakDemographic
                            {
                                Break = aBreak,
                                Demographic = bd.Demographic,
                                Rating = bd.Rating
                            }));

                allBreakCommercials.Add(
                    new BreakCommercials
                    {
                        Break = aBreak,
                        Commercials = new List<Commercial>()
                    });
            }

            orderedBreakDemographics.Sort();

            //We will fill the commercial slots until none are left
            while (HasSpaceAvailable(allBreakCommercials) &&
                   commercials.Any())
            {
                bool addedCommercial = false;

                foreach (var orderedBreakDemographic in orderedBreakDemographics)
                {
                    var matchingBreakCommercials =
                        allBreakCommercials.FirstOrDefault(
                            value => value.Break.Id == orderedBreakDemographic.Break.Id);
                    if (matchingBreakCommercials == null) continue;

                    if (matchingBreakCommercials.Commercials.Count == matchingBreakCommercials.Break.Capacity)
                        //We've already filled the break
                        continue;

                    //Fill the commercials in the best positions, ignoring for now the commercial type and 
                    //associated restrictions
                    var matchingCommercial =
                        commercials.FirstOrDefault(
                            commercial =>
                                commercial.Demographic.Id == orderedBreakDemographic.Demographic.Id);

                    if (matchingCommercial != null)
                    {
                        //Allocate the commercial
                        matchingBreakCommercials.Commercials.Add(matchingCommercial);
                        commercials.Remove(matchingCommercial);
                        addedCommercial = true;
                    }
                }

                if (!addedCommercial) break;
            }

            //Failed to fill the break commercial slots
            if (HasSpaceAvailable(allBreakCommercials))
                throw new ArgumentException("Unable to fill the breaks with the available commercials");

            LogCurrentAllocation(allBreakCommercials);

            //Now we must deal with commercial type restrictions - 
            //We do this by finding the best valid swap of commercials from one break to another
            //based on the change in rating.
            //Do this until there are no more restriction conflicts or we can't do a swap
            SwapCommercials(allBreakCommercials, commercials);

            //Finally we should be left with breaks filled with commercials can be rearranged into
            //a valid order - so that's what we do next
            ReorderCommercials(allBreakCommercials);

            if (!CheckTypesAreValid(allBreakCommercials))
                throw new ArgumentException(
                    "Unable to fill the breaks with the available commercials due to commercial type restrictions");

            return allBreakCommercials;
        }

        #endregion

        #region Private Methods

        private bool CheckTypesAreValid(List<BreakCommercials> allBreakCommercials)
        {
            foreach (var breakCommercials in allBreakCommercials)
            {
                string previousCommercialType = null;

                foreach (var breakCommercial in breakCommercials.Commercials)
                {
                    if (breakCommercials.Break.InvalidCommercialTypes != null &&
                        breakCommercials.Break.InvalidCommercialTypes.Contains(
                            breakCommercial.CommercialType))
                        return false;

                    if (!string.IsNullOrEmpty(previousCommercialType) &&
                        breakCommercial.CommercialType == previousCommercialType)
                        return false;

                    previousCommercialType = breakCommercial.CommercialType;
                }
            }

            return true;
        }

        private bool IsCommercialValidForBreak(
            BreakCommercials breakCommercials,
            Commercial commercial)
        {
            //Is the source commercial valid for the target break?
            if (breakCommercials.Break.InvalidCommercialTypes != null &&
                breakCommercials.Break.InvalidCommercialTypes.Contains(commercial.CommercialType))
                //Commercial type is invalid for this break
                return false;

            var targetCountAlreadyInBreak =
                breakCommercials.Commercials.Count(
                    value => value.CommercialType == commercial.CommercialType);
            if (targetCountAlreadyInBreak + 1 > MaxBreakCapacityForCommercialType(breakCommercials.Break))
                //Adding this type to the break will mean there's too many of the type 
                return false;

            return true;
        }

        private int GetRatingChangeForSwap(
            BreakCommercials sourceBreakCommercials,
            Commercial sourceCommercial,
            BreakCommercials targetBreakCommercials,
            Commercial targetCommercial)
        {
            var ratingTotalAfterSwap = 0;
            
            if (sourceBreakCommercials != null)
                ratingTotalAfterSwap += sourceBreakCommercials.Break.BreakDemographics
                    .Where(value => value.Demographic.Id == targetCommercial.Demographic.Id)
                    .Select(value => value.Rating).FirstOrDefault();

            if (targetBreakCommercials != null)
                ratingTotalAfterSwap += targetBreakCommercials.Break.BreakDemographics
                    .Where(value => value.Demographic.Id == sourceCommercial.Demographic.Id)
                    .Select(value => value.Rating).FirstOrDefault();

            var ratingTotalBeforeSwap = 0;

            if (sourceBreakCommercials != null)
                ratingTotalAfterSwap += sourceBreakCommercials.Break.BreakDemographics
                    .Where(value => value.Demographic.Id == sourceCommercial.Demographic.Id)
                    .Select(value => value.Rating).FirstOrDefault();

            if (targetBreakCommercials != null)
                ratingTotalAfterSwap += targetBreakCommercials.Break.BreakDemographics
                    .Where(value => value.Demographic.Id == targetCommercial.Demographic.Id)
                    .Select(value => value.Rating).FirstOrDefault();

            return ratingTotalAfterSwap - ratingTotalBeforeSwap;
        }

        private (
            BreakCommercials Commercials, 
            Commercial NewSourceCommercial, 
            Commercial OldTargetCommercial,
            Commercial NewTargetCommercial,
            int RatingChange)? 
            GetBestSwap(
                BreakCommercials sourceBreakCommercials,
                Commercial sourceCommercial,
                List<BreakCommercials> otherBreakCommercials,
                List<Commercial> unusedCommercials)
        {
            var bestRatingChange = int.MinValue;
            BreakCommercials selectedBreakCommercials = null;
            Commercial selectedOldTargetCommercial = null;
            Commercial selectedNewTargetCommercial = null;
            Commercial selectedNewSourceCommercial = sourceCommercial;

            //Check the rating change if we swap the source commercial with an unused one
            foreach (var unusedCommercial in unusedCommercials)
            {
                var ratingChange =
                    GetRatingChangeForSwap(
                        sourceBreakCommercials,
                        sourceCommercial,
                        null,
                        unusedCommercial);

                if (ratingChange > bestRatingChange)
                {
                    bestRatingChange = ratingChange;
                    selectedNewSourceCommercial = unusedCommercial;
                }
            }

            //Create list of possible swaps, ignoring commercials which are invalid
            //or of the same type
            foreach (var breakCommercials in otherBreakCommercials)
            {
                //Is the source commercial valid for the target break?
                if (!IsCommercialValidForBreak(breakCommercials, sourceCommercial))
                    continue;

                foreach (var targetCommercial in breakCommercials.Commercials)
                {
                    //Is the target commercial valid for the source break?
                    if (!IsCommercialValidForBreak(sourceBreakCommercials, targetCommercial))
                        continue;

                    //What's the rating total change after the swap?
                    var ratingChange =
                        GetRatingChangeForSwap(
                            sourceBreakCommercials,
                            sourceCommercial,
                            breakCommercials,
                            targetCommercial);

                    //Check the rating change if we swap the target commercial with an unused one
                    Commercial selectedUnusedCommercial = null;
                    int unusedCommercialRatingChange = int.MinValue;

                    foreach (var unusedCommercial in unusedCommercials)
                    {
                        var unusedRatingChange =
                            GetRatingChangeForSwap(
                                null,
                                unusedCommercial,
                                breakCommercials,
                                targetCommercial);

                        if (unusedRatingChange > unusedCommercialRatingChange)
                        {
                            unusedCommercialRatingChange = unusedRatingChange;
                            selectedUnusedCommercial = unusedCommercial;
                        }
                    }

                    if (unusedCommercialRatingChange > 0)
                    {
                        ratingChange += unusedCommercialRatingChange;
                    }

                    if (bestRatingChange < ratingChange)
                    {
                        bestRatingChange = ratingChange;
                        selectedBreakCommercials = breakCommercials;
                        selectedNewSourceCommercial = targetCommercial;
                        selectedOldTargetCommercial = targetCommercial;

                        if (unusedCommercialRatingChange > 0)
                        {
                            selectedNewTargetCommercial = selectedUnusedCommercial;
                        }
                        else
                        {
                            selectedNewTargetCommercial = sourceCommercial;
                        }
                    }
                }
            }

            return (
                selectedBreakCommercials,
                selectedNewSourceCommercial, 
                selectedOldTargetCommercial,
                selectedNewTargetCommercial,
                bestRatingChange);
        }

        private bool HasSpaceAvailable(List<BreakCommercials> allBreakCommercials)
        {
            var hasSpace =
                allBreakCommercials.Any(
                    breakCommercials =>
                        breakCommercials.Commercials.Count < breakCommercials.Break.Capacity);
            return hasSpace;
        }

        private void LogCurrentAllocation(List<BreakCommercials> allBreakCommercials)
        {
            Debug.WriteLine("Break Commercials:");
            foreach (var breakCommercials in allBreakCommercials)
            {
                Debug.WriteLine(
                    string.Join(
                        ", ",
                        breakCommercials.Commercials.Select(value => value.Id)));
            }
        }

        private int MaxBreakCapacityForCommercialType(Break aBreak)
        {
            return (int) Math.Ceiling(aBreak.Capacity / 2d);
        }

        private void ReorderCommercials(List<BreakCommercials> allBreakCommercials)
        {
            foreach (var breakCommercials in allBreakCommercials)
            {
                var reorderedCommercials = new List<Commercial>();

                var frequentCommercialType = GetMostFrequentCommercialType(breakCommercials);

                bool doneReordering = true;
                while (breakCommercials.Commercials.Count > 0 &&
                       doneReordering)
                {
                    doneReordering = false;

                    Commercial matchingCommercial = null;
                    
                    var lastCommercialType = reorderedCommercials.LastOrDefault()?.CommercialType;
                    if (lastCommercialType == null || lastCommercialType != frequentCommercialType.Item1)
                    {
                        //Prioritise adding a commercial of the most frequent type to avoid 
                        //possible adjacency conflicts later
                        matchingCommercial = 
                            breakCommercials.Commercials.FirstOrDefault(value => 
                                value.CommercialType == frequentCommercialType.Item1);
                    }

                    if (matchingCommercial == null)
                    {
                        foreach (var commercial in breakCommercials.Commercials)
                        {
                            if (!reorderedCommercials.Any() ||
                                reorderedCommercials.Last().CommercialType != commercial.CommercialType)
                            {
                                matchingCommercial = commercial;
                                break;
                            }
                        }
                    }

                    if (matchingCommercial != null)
                    {
                        reorderedCommercials.Add(matchingCommercial);
                        breakCommercials.Commercials.Remove(matchingCommercial);
                        doneReordering = true;
                    }
                }

                breakCommercials.Commercials = reorderedCommercials;
            }
        }

        private (string, int) GetMostFrequentCommercialType(
            BreakCommercials breakCommercials)
        {
            var commercialFrequency =
                breakCommercials.Commercials
                    .GroupBy(c => c.CommercialType)
                    .Select(
                        g => new
                        {
                            CommercialType = g.Key,
                            Count = g.Select(c => c.CommercialType).Count()
                        });
            var maxValue =
                commercialFrequency.First(value1 => 
                    value1.Count == commercialFrequency.Max(value2 => value2.Count));
            return (maxValue.CommercialType, maxValue.Count);
        }

        private void SwapCommercials(
            List<BreakCommercials> allBreakCommercials,
            List<Commercial> unusedCommercials)
        {
            foreach (var breakCommercials in allBreakCommercials)
            {
                //If there's any commercials of an invalid type, they have to go
                if (breakCommercials.Break.InvalidCommercialTypes != null)
                {
                    var invalidCommercials = breakCommercials.Commercials.Where(
                        value =>
                            breakCommercials.Break.InvalidCommercialTypes.Contains(value.CommercialType)).ToList();

                    foreach (var invalidCommercial in invalidCommercials)
                    {
                        var bestSwapResult =
                            GetBestSwap(
                                breakCommercials,
                                invalidCommercial,
                                allBreakCommercials.Where(
                                    value => value.Break.Id != breakCommercials.Break.Id).ToList(),
                                unusedCommercials);
                        if (bestSwapResult.HasValue)
                        {
                            var selectedBreakCommercials = bestSwapResult.Value.Item1;
                            var selectedTargetCommercial = bestSwapResult.Value.Item2;
                            selectedBreakCommercials.Commercials.Remove(selectedTargetCommercial);
                            selectedBreakCommercials.Commercials.Add(invalidCommercial);
                            breakCommercials.Commercials.Remove(invalidCommercial);
                            breakCommercials.Commercials.Add(selectedTargetCommercial);

                            Debug.WriteLine($"Swapped {invalidCommercial.Id} for {selectedTargetCommercial.Id}");
                        }
                        else
                        {
                            throw new ArgumentException(
                                "Unable to fill the breaks with the available commercials - cannot reallocate invalid commercial");
                        }
                    }
                }

                //Next check if there's too many commercials of a certain type 
                //(e.g. if we have 4 slots and 3 of a certain type, there's no way
                //to arrange them so they aren't adjacent). In this case we will go through
                //comparing the rating change for the potential swaps and pick the most favourable one
                var frequentCommercialType = GetMostFrequentCommercialType(breakCommercials);

                string tooFrequentCommercialType = null;
                int maxFrequencyAllowed = MaxBreakCapacityForCommercialType(breakCommercials.Break);

                if (frequentCommercialType.Item2 > maxFrequencyAllowed)
                    tooFrequentCommercialType = frequentCommercialType.Item1;

                if (!string.IsNullOrEmpty(tooFrequentCommercialType))
                {
                    var matchingCommercials =
                        breakCommercials.Commercials.Where(
                            value =>
                                value.CommercialType == tooFrequentCommercialType).ToList();

                    while (matchingCommercials.Count > maxFrequencyAllowed)
                    {
                        var bestRatingChange = int.MinValue;
                        BreakCommercials selectedBreakCommercials = null;
                        Commercial oldSourceCommercial = null;
                        Commercial newSourceCommercial = null;
                        Commercial oldTargetCommercial = null;
                        Commercial newTargetCommercial = null;

                        foreach (var matchingCommercial in matchingCommercials)
                        {
                            var bestSwapResult =
                                GetBestSwap(
                                    breakCommercials,
                                    matchingCommercial,
                                    allBreakCommercials.Where(
                                        value => value.Break.Id != breakCommercials.Break.Id).ToList(),
                                    unusedCommercials);
                            if (bestSwapResult.HasValue && bestSwapResult.Value.RatingChange > bestRatingChange)
                            {
                                oldSourceCommercial = matchingCommercial;
                                newSourceCommercial = bestSwapResult.Value.NewSourceCommercial;
                                oldTargetCommercial = bestSwapResult.Value.OldTargetCommercial;
                                newTargetCommercial = bestSwapResult.Value.NewTargetCommercial;
                                selectedBreakCommercials = bestSwapResult.Value.Item1;
                                bestRatingChange = bestSwapResult.Value.RatingChange;

                                if (oldSourceCommercial != newSourceCommercial)
                                    Debug.WriteLine(
                                        $"Swapped {oldSourceCommercial.Id} for {newSourceCommercial.Id}");
                                if (oldTargetCommercial != newTargetCommercial)
                                    Debug.WriteLine(
                                        $"Swapped {oldTargetCommercial.Id} for {newTargetCommercial.Id}");
                            }
                        }
                        
                        //If we have a valid swap setup, do it
                        if (newSourceCommercial == null ||
                            newSourceCommercial == oldSourceCommercial)
                        {
                            throw new ArgumentException(
                                "Unable to fill the breaks with the available commercials - cannot reallocate too-frequent commercial");
                        }

                        breakCommercials.Commercials.Remove(oldSourceCommercial);
                        breakCommercials.Commercials.Add(newSourceCommercial);
                        selectedBreakCommercials.Commercials.Remove(oldTargetCommercial);
                        selectedBreakCommercials.Commercials.Add(newTargetCommercial);
                        matchingCommercials.Remove(oldSourceCommercial);
                    }
                }
            }
        }

        #endregion

        #region Nested type: OrderedBreakDemographic

        private class OrderedBreakDemographic : IComparable<OrderedBreakDemographic>
        {
            #region Public Properties

            public Break Break { get; set; }
            public Demographic Demographic { get; set; }
            public int Rating { get; set; }

            #endregion

            #region Public Methods

            public int CompareTo(OrderedBreakDemographic demo)
            {
                return demo.Rating.CompareTo(Rating);
            }

            #endregion
        }

        #endregion
    }
}