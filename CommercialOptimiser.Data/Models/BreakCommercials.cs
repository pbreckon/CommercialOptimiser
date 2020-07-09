using System;
using System.Collections.Generic;
using System.Text;

namespace CommercialOptimiser.Data.Models
{
    public class BreakCommercials
    {
        public Break Break { get; set; }

        public List<Commercial> Commercials { get; set; }
    }
}
