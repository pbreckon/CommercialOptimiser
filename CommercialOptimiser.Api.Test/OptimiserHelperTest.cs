using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CommercialOptimiser.Api.Database.Tables;
using CommercialOptimiser.Api.Helpers;
using CommercialOptimiser.Data.Models;
using NUnit.Framework;

namespace CommercialOptimiser.Api.Test
{
    public class Tests
    {
        private List<Demographic> _demographics;
        private List<Break> _breaks;
        private List<Commercial> _commercials;

        [SetUp]
        public void Setup()
        {
            _demographics =
               new List<Demographic>
               {
                    new Demographic {Id = 1, Title = "W25-30"},
                    new Demographic {Id = 2, Title = "M18-35"},
                    new Demographic {Id = 3, Title = "T18-40"}
               };

            _breaks =
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
                                        Demographic = _demographics[0],
                                        Rating = 80
                                    },
                                    new BreakDemographic
                                    {
                                        Demographic = _demographics[1],
                                        Rating = 100
                                    },
                                    new BreakDemographic
                                    {
                                        Demographic = _demographics[2],
                                        Rating = 250
                                    }
                                }
                        },
                    new Break
                        {
                            Id = 2,
                            Title = "Break2",
                            Capacity = 3,
                            InvalidCommercialTypes = new List<string> { "Finance" },
                            BreakDemographics =
                                new List<BreakDemographic>
                                {
                                    new BreakDemographic
                                    {
                                        Demographic = _demographics[0],
                                        Rating = 50
                                    },
                                    new BreakDemographic
                                    {
                                        Demographic = _demographics[1],
                                        Rating = 120
                                    },
                                    new BreakDemographic
                                    {
                                        Demographic = _demographics[2],
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
                                    Demographic = _demographics[0],
                                    Rating = 350
                                },
                                new BreakDemographic
                                {
                                    Demographic = _demographics[1],
                                    Rating = 150
                                },
                                new BreakDemographic
                                {
                                    Demographic = _demographics[2],
                                    Rating = 500
                                }
                            }
                    }
                };

            _commercials =
                new List<Commercial>
                {
                    new Commercial
                    {
                        Id = 1, Title = "Commercial 1", CommercialType = "Automotive", Demographic = _demographics[0]
                    },
                    new Commercial
                    {
                        Id = 2, Title = "Commercial 2", CommercialType = "Travel", Demographic = _demographics[1]
                    },
                    new Commercial
                    {
                        Id = 3, Title = "Commercial 3", CommercialType = "Travel", Demographic = _demographics[2]
                    },
                    new Commercial
                    {
                        Id = 4, Title = "Commercial 4", CommercialType = "Automotive", Demographic = _demographics[1]
                    },
                    new Commercial
                    {
                        Id = 5, Title = "Commercial 5", CommercialType = "Automotive", Demographic = _demographics[1]
                    },
                    new Commercial
                    {
                        Id = 6, Title = "Commercial 6", CommercialType = "Finance", Demographic = _demographics[0]
                    },
                    new Commercial
                    {
                        Id = 7, Title = "Commercial 7", CommercialType = "Finance", Demographic = _demographics[1]
                    },
                    new Commercial
                    {
                        Id = 8, Title = "Commercial 8", CommercialType = "Automotive", Demographic = _demographics[2]
                    },
                    new Commercial
                    {
                        Id = 9, Title = "Commercial 9", CommercialType = "Travel", Demographic = _demographics[0]
                    }
                };
        }

        [Test]
        public void TestOptimalPlacementForNineCommercialsWithStandardCapacity()
        {
            var optimiser = new OptimiserHelper();

            var optimisedBreakCommercials = 
                optimiser.GetOptimalBreakCommercials(_breaks, _commercials);

            Assert.IsTrue(optimisedBreakCommercials != null && optimisedBreakCommercials.Count == 3);
            Assert.AreEqual(1, optimisedBreakCommercials[0].Break?.Id);
            Assert.AreEqual(3, optimisedBreakCommercials[0].Commercials.Count);
            var commercialIds = optimisedBreakCommercials[0].Commercials.Select(value => value.Id).ToList();
            commercialIds.Sort();
            Assert.IsTrue(commercialIds.SequenceEqual(new List<int> { 6, 7, 8 }));

            Assert.IsTrue(optimisedBreakCommercials[1].Break?.Id == 2);
            Assert.IsTrue(optimisedBreakCommercials[1].Commercials.Count == 3);
            commercialIds = optimisedBreakCommercials[1].Commercials.Select(value => value.Id).ToList();
            commercialIds.Sort();
            Assert.IsTrue(commercialIds.SequenceEqual(new List<int> { 4, 5, 9 }));
            
            Assert.IsTrue(optimisedBreakCommercials[2].Break?.Id == 3);
            Assert.IsTrue(optimisedBreakCommercials[2].Commercials.Count == 3);
            commercialIds = optimisedBreakCommercials[2].Commercials.Select(value => value.Id).ToList();
            commercialIds.Sort();
            Assert.IsTrue(commercialIds.SequenceEqual(new List<int> { 1, 2, 3 }));

            CheckTypesAreValid(optimisedBreakCommercials);
        }

        [Test]
        public void TestOptimalPlacementForTenCommercialsWithStandardCapacity()
        {
            var optimiser = new OptimiserHelper();

            _commercials.Add(
                new Commercial
                {
                    Id = 10,
                    Title = "Commercial 10",
                    CommercialType = "Finance",
                    Demographic = _demographics[2]
                });
            
            var optimisedBreakCommercials =
                optimiser.GetOptimalBreakCommercials(_breaks, _commercials);

            Assert.IsTrue(optimisedBreakCommercials != null && optimisedBreakCommercials.Count == 3);
            Assert.AreEqual(1, optimisedBreakCommercials[0].Break?.Id);
            Assert.AreEqual(3, optimisedBreakCommercials[0].Commercials.Count);
            var commercialIds = optimisedBreakCommercials[0].Commercials.Select(value => value.Id).ToList();
            commercialIds.Sort();
            Assert.IsTrue(commercialIds.SequenceEqual(new List<int> { 6, 7, 8 }));

            Assert.IsTrue(optimisedBreakCommercials[1].Break?.Id == 2);
            Assert.IsTrue(optimisedBreakCommercials[1].Commercials.Count == 3);
            commercialIds = optimisedBreakCommercials[1].Commercials.Select(value => value.Id).ToList();
            commercialIds.Sort();
            Assert.IsTrue(commercialIds.SequenceEqual(new List<int> { 2, 4, 6 }));

            Assert.IsTrue(optimisedBreakCommercials[2].Break?.Id == 3);
            Assert.IsTrue(optimisedBreakCommercials[2].Commercials.Count == 3);
            commercialIds = optimisedBreakCommercials[2].Commercials.Select(value => value.Id).ToList();
            commercialIds.Sort();
            Assert.IsTrue(commercialIds.SequenceEqual(new List<int> { 1, 3, 10 }));

            CheckTypesAreValid(optimisedBreakCommercials);
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
    }
}