using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Data.Tables
{
    [Table("UserReportBreakCommercial")]
    public class UserReportBreakCommercialTable : IBaseTable
    {
        #region Public Properties
        
        public string CommercialTitle { get; set; }

        [Column(nameof(Id))]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Rating { get; set; }

        public virtual UserReportBreakTable UserReportBreak { get; set; }

        #endregion
    }
}