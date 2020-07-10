using System;
using System.Collections.Generic;
using System.Linq;
using CommercialOptimiser.Api.Helpers;
using CommercialOptimiser.Data.Models;
using NUnit.Framework;

namespace CommercialOptimiser.Api.Test
{
    public class Tests
    {
        #region Public Methods

        [Test]
        public void TestOptimalPlacementForNineCommercialsWithStandardCapacity()
        {
            var (breaks, commercials, _) = GetBasicData();

            var optimiser = new OptimiserHelper();

            var optimisedBreakCommercials =
                optimiser.GetOptimalBreakCommercials(breaks, commercials);

            CheckResults(optimisedBreakCommercials, breaks, commercials, 1970);
        }

        [Test]
        public void TestOptimalPlacementForEdgeCases()
        {
            var optimiser = new OptimiserHelper();
            var (breaks, commercials, _) = GetBasicData();

            commercials.RemoveAt(0);
            List<BreakCommercials> optimisedBreakCommercials;

            try
            {
                optimisedBreakCommercials =
                    optimiser.GetOptimalBreakCommercials(breaks, commercials);
                Assert.Fail("Shouldn't be enough commercials");
            }
            catch (ArgumentException)
            {
            }

            (breaks, commercials, _) = GetBasicData();
            commercials[0].CommercialType = CommercialType.Finance.ToString();
            commercials[1].CommercialType = CommercialType.Finance.ToString();

            //4 financial commercials, should still be possible
            optimisedBreakCommercials =
                optimiser.GetOptimalBreakCommercials(breaks, commercials);
            CheckResults(optimisedBreakCommercials, breaks, commercials, 1940);

            commercials[2].CommercialType = CommercialType.Finance.ToString();

            try
            {
                optimisedBreakCommercials =
                    optimiser.GetOptimalBreakCommercials(breaks, commercials);
                Assert.Fail("Too many financial commercials, one break would contain three");
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void TestOptimalPlacementForNineCommercialsWithVariableCapacity()
        {
            var optimiser = new OptimiserHelper();
            var (breaks, commercials, _) = GetBasicData();

            breaks[0].Capacity = 2;
            breaks[1].Capacity = 3;
            breaks[2].Capacity = 4;

            var optimisedBreakCommercials =
                optimiser.GetOptimalBreakCommercials(breaks, commercials);

            CheckResults(optimisedBreakCommercials, breaks, commercials, 2240);

            breaks[0].Capacity = 4;
            breaks[1].Capacity = 3;
            breaks[2].Capacity = 2;

            optimisedBreakCommercials =
                optimiser.GetOptimalBreakCommercials(breaks, commercials);

            CheckResults(optimisedBreakCommercials, breaks, commercials, 1700);
        }

        [Test]
        public void TestOptimalPlacementForTenCommercialsWithStandardCapacity()
        {
            var optimiser = new OptimiserHelper();
            var (breaks, commercials, demographics) = GetBasicData();

            commercials.Add(
                new Commercial
                {
                    Id = 10,
                    Title = "Commercial 10",
                    CommercialType = "Finance",
                    Demographic = demographics[2]
                });

            var optimisedBreakCommercials =
                optimiser.GetOptimalBreakCommercials(breaks, commercials);

            CheckResults(optimisedBreakCommercials, breaks, commercials, 2120);
        }

        #endregion

        #region Private Methods

        private void CheckResults(
            List<BreakCommercials> optimisedBreakCommercials,
            List<Break> breaks,
            List<Commercial> commercials,
            int expectedTotalRating)
        {
            Assert.IsTrue(
                optimisedBreakCommercials != null &&
                optimisedBreakCommercials.Count == breaks.Count);

            for (int i = 0; i < optimisedBreakCommercials.Count; i++)
            {
                Assert.IsTrue(optimisedBreakCommercials[i].Break?.Id == i + 1);
                Assert.IsTrue(optimisedBreakCommercials[i].Commercials.Count == breaks[i].Capacity);
            }

            CheckTypesAreValid(optimisedBreakCommercials);

            //There are multiple combinations which can result in the maximum rating, so we just
            //check we've got the best rating available
            var totalRating = GetRating(optimisedBreakCommercials);
            Assert.AreEqual(expectedTotalRating, totalRating);
        }

        private void CheckTypesAreValid(List<BreakCommercials> allBreakCommercials)
        {
            foreach (var breakCommercials in allBreakCommercials)
            {
                string previousCommercialType = null;

                foreach (var breakCommercial in breakCommercials.Commercials)
                {
                    Assert.IsTrue(
                        breakCommercials.Break.InvalidCommercialTypes == null ||
                        !breakCommercials.Break.InvalidCommercialTypes.Contains(
                            breakCommercial.CommercialType));

                    Assert.IsTrue(
                        string.IsNullOrEmpty(previousCommercialType) ||
                        breakCommercial.CommercialType != previousCommercialType);

                    previousCommercialType = breakCommercial.CommercialType;
                }
            }
        }

        private (List<Break>, List<Commercial>, List<Demographic>) GetBasicData()
        {
            var demographics =
                new List<Demographic>
                {
                    new Demographic {Id = 1, Title = "W25-30"},
                    new Demographic {Id = 2, Title = "M18-35"},
                    new Demographic {Id = 3, Title = "T18-40"}
                };

            var breaks =
                new List<Break>
                {
                    new Break
                    {
                        Id = 1,
                        Title = "Break1",
                        Capacity = 3,
                        BreakDemographics =
                            new List<BreakDemographic>
                            {
                                new BreakDemographic
                                {
                                    Demographic = demographics[0],
                                    Rating = 80
                                },
                                new BreakDemographic
                                {
                                    Demographic = demographics[1],
                                    Rating = 100
                                },
                                new BreakDemographic
                                {
                                    Demographic = demographics[2],
                                    Rating = 250
                                }
                            }
                    },
                    new Break
                    {
                        Id = 2,
                        Title = "Break2",
                        Capacity = 3,
                        InvalidCommercialTypes = new List<string> {"Finance"},
                        BreakDemographics =
                            new List<BreakDemographic>
                            {
                                new BreakDemographic
                                {
                                    Demographic = demographics[0],
                                    Rating = 50
                                },
                                new BreakDemographic
                                {
                                    Demographic = demographics[1],
                                    Rating = 120
                                },
                                new BreakDemographic
                                {
                                    Demographic = demographics[2],
                                    Rating = 200
                                }
                            }
                    },
                    new Break
                    {
                        Id = 3,
                        Title = "Break3",
                        Capacity = 3,
                        BreakDemographics =
                            new List<BreakDemographic>
                            {
                                new BreakDemographic
                                {
                                    Demographic = demographics[0],
                                    Rating = 350
                                },
                                new BreakDemographic
                                {
                                    Demographic = demographics[1],
                                    Rating = 150
                                },
                                new BreakDemographic
                                {
                                    Demographic = demographics[2],
                                    Rating = 500
                                }
                            }
                    }
                };

            var commercials =
                new List<Commercial>
                {
                    new Commercial
                    {
                        Id = 1, Title = "Commercial 1", CommercialType = "Automotive", Demographic = demographics[0]
                    },
                    new Commercial
                    {
                        Id = 2, Title = "Commercial 2", CommercialType = "Travel", Demographic = demographics[1]
                    },
                    new Commercial
                    {
                        Id = 3, Title = "Commercial 3", CommercialType = "Travel", Demographic = demographics[2]
                    },
                    new Commercial
                    {
                        Id = 4, Title = "Commercial 4", CommercialType = "Automotive", Demographic = demographics[1]
                    },
                    new Commercial
                    {
                        Id = 5, Title = "Commercial 5", CommercialType = "Automotive", Demographic = demographics[1]
                    },
                    new Commercial
                    {
                        Id = 6, Title = "Commercial 6", CommercialType = "Finance", Demographic = demographics[0]
                    },
                    new Commercial
                    {
                        Id = 7, Title = "Commercial 7", CommercialType = "Finance", Demographic = demographics[1]
                    },
                    new Commercial
                    {
                        Id = 8, Title = "Commercial 8", CommercialType = "Automotive", Demographic = demographics[2]
                    },
                    new Commercial
                    {
                        Id = 9, Title = "Commercial 9", CommercialType = "Travel", Demographic = demographics[0]
                    }
                };

            return (breaks, commercials, demographics);
        }

        private int GetRating(List<BreakCommercials> breakCommercials)
        {
            int totalRating = 0;
            foreach (var breakCommercial in breakCommercials)
            {
                foreach (var commercial in breakCommercial.Commercials)
                {
                    var matchingBreak =
                        breakCommercial.Break.BreakDemographics.FirstOrDefault(
                            value => value.Demographic.Id == commercial.Demographic.Id);
                    Assert.IsTrue(matchingBreak != null);

                    totalRating += matchingBreak.Rating;
                }
            }

            return totalRating;
        }

        #endregion
    }
}