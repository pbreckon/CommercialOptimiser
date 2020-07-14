using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Data.Tables
{
    [Table("UserReportBreak")]
    public class UserReportBreakTable
    {
        #region Public Properties

        [Column(nameof(UserUniqueId))]
        [Key]
        public string UserUniqueId { get; set; }

        public string Report { get; set; }

        #endregion
    }
}