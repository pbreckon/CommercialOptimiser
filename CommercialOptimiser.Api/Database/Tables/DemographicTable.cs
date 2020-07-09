using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Api.Database.Tables
{
    [Table("Demographic", Schema = "dbo")]
    public class DemographicTable
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Title")]
        [MaxLength(255)]
        public string Title { get; set; }
    }
}
