using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Data.Tables
{
    [Table("Break")]
    public class BreakTable : IBaseTable
    {
        #region Public Properties

        public virtual List<BreakDemographicTable> BreakDemographics { get; set; } = new List<BreakDemographicTable>();

        [Column(nameof(Capacity))]
        public int Capacity { get; set; }

        [Column(nameof(Id))]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(nameof(InvalidCommercialTypes))]
        public string InvalidCommercialTypes { get; set; }

        [Column(nameof(Title))]
        [StringLength(255)]
        [Required]
        public string Title { get; set; }

        #endregion
    }
}