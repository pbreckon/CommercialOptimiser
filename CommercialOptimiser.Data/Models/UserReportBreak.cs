using System;
using System.Collections.Generic;
using System.Text;

namespace CommercialOptimiser.Core.Models
{
    public class UserReportBreak
    {
        #region Public Properties

        public int Id { get; set; }

        public string BreakTitle { get; set; }

        public User User { get; set; }

        public List<UserReportBreakCommercial> UserReportBreakCommercials { get; set; }

        #endregion
    }
}