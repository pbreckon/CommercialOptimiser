using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Api.Database.Tables
{
    [Table("Break")]
    public class BreakTable
    {
        #region Public Properties

        public virtual List<BreakDemographicTable> BreakDemographics { get; set; } = new List<BreakDemographicTable>();

        [Column("Capacity")]
        public int Capacity { get; set; }

        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("InvalidCommercialTypes")]
        public string InvalidCommercialTypes { get; set; }

        [Column("Title")]
        [StringLength(255)]
        [Required]
        public string Title { get; set; }

        #endregion
    }
}