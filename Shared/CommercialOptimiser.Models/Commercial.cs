using System;
using System.Collections.Generic;
using System.Text;

namespace CommercialOptimiser.Models
{
    public class Commercial
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public CommercialType Type { get; set; }

        public Demographic Demographic { get; set; }
    }
}
