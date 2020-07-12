using System;
using System.Collections.Generic;
using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.Core.Requests
{
    public class CalculateOptimalBreakCommercialsRequest
    {
        #region Public Properties

        public List<Commercial> Commercials { get; set; }
        public string UserUniqueId { get; set; }

        #endregion
    }
}