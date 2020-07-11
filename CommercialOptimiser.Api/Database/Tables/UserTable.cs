using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialOptimiser.Api.Database.Tables
{
    [Table("User")]
    public class UserTable : IBaseTable
    {
        #region Public Properties

        [Column(nameof(Id))]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(nameof(UserUniqueId))]
        public string UserUniqueId { get; set; }

        #endregion
    }
}