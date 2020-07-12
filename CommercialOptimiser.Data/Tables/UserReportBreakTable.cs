using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Data.Tables
{
    [Table("UserReportBreak")]
    public class UserReportBreakTable : IBaseTable
    {
        #region Public Properties

        public string BreakTitle { get; set; }

        public virtual List<UserReportBreakCommercialTable> UserReportBreakCommercials { get; set; }

        [Column(nameof(Id))]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual UserTable User { get; set; }

        #endregion
    }
}