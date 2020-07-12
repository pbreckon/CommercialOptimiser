using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Data.Tables
{
    [Table("Demographic")]
    public class DemographicTable : IBaseTable
    {
        #region Public Properties

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