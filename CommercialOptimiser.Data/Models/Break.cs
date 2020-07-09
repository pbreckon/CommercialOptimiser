using System.Collections.Generic;

namespace CommercialOptimiser.Data.Models
{
    public class Break
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<BreakDemographic> BreakDemographics { get; set; }
            
        public int Capacity { get; set; }

        public List<string> InvalidCommercialTypes { get; set; }
    }
}
