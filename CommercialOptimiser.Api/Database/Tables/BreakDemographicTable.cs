using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Api.Database.Tables
{
    [Table("BreakDemographic", Schema = "dbo")]
    public class BreakDemographicTable
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual BreakTable Break { get; set; }

        public virtual DemographicTable Demographic { get; set; }

        [Column("Rating")]
        public int Rating { get; set; }
    }
}
