using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Data.Tables
{
    [Table("User")]
    public class UserTable : IBaseTable
    {
        #region Public Properties

        [Column(nameof(Id))]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(nameof(UniqueUserId))]
        public string UniqueUserId { get; set; }

        #endregion
    }
}