using System;
using System.Collections.Generic;

namespace CommercialOptimiser.Models
{
    public class Break
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public List<BreakDemographic> BreakDemographics { get; set; }

        public int Capacity { get; set; }
    }
}
