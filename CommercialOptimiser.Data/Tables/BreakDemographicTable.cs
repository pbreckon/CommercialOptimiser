using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Data.Tables
{
    [Table("BreakDemographic")]
    public class BreakDemographicTable
    {
        [Column(nameof(Id))]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual BreakTable Break { get; set; }

        public virtual DemographicTable Demographic { get; set; }

        [Column(nameof(Rating))]
        public int Rating { get; set; }
    }
}
