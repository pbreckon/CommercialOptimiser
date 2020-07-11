using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Api.Database.Tables
{
    [Table("Commercial")]
    public class CommercialTable : IBaseTable
    {
        #region Public Properties

        [Column(nameof(CommercialType))]
        [MaxLength(50)]
        public string CommercialType { get; set; }

        public virtual DemographicTable Demographic { get; set; }
        
        [Column(nameof(Id))]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(nameof(Title))]
        [MaxLength(255)]
        public string Title { get; set; }

        #endregion
    }
}