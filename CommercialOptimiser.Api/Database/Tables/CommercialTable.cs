using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommercialOptimiser.Data.Models;

namespace CommercialOptimiser.Api.Database.Tables
{
    [Table("Commercial", Schema = "dbo")]
    public class CommercialTable
    {
        #region Public Properties

        [Column("CommercialType")]
        [MaxLength(50)]
        public string CommercialType { get; set; }

        public virtual DemographicTable Demographic { get; set; }
        
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Title")]
        [MaxLength(255)]
        public string Title { get; set; }

        #endregion
    }
}