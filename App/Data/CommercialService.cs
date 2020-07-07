using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialOptimiser.Models;

namespace CommercialOptimiser.Data
{
    public class CommercialService
    {
        public async Task<List<Break>> GetBreaksAsync() 
        {
            return
                new List<Break>
                {
                    new Break {
                        Id = 1,
                        Name = "Break1",
                        Capacity = 3,
                        BreakDemographics =
                            new List<BreakDemographic>
                            {
                                new BreakDemographic
                                {
                                    Demographic = new Demographic { Name = "W25-30" },
                                    Rating = 80
                                },
                                new BreakDemographic
                                {
                                    Demographic = new Demographic { Name = "M18-35" },
                                    Rating = 100
                                },
                                new BreakDemographic
                                {
                                    Demographic = new Demographic { Name = "T18-40" },
                                    Rating = 250
                                }
                            }
                    },
                    new Break {
                        Id = 2,
                        Name = "Break2",
                        Capacity = 3
                    },
                    new Break { Id = 3, Name = "Break3", Capacity = 3 }
                };
        }
    }
}
