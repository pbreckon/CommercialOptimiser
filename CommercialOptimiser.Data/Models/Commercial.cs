
namespace CommercialOptimiser.Data.Models
{
    public class Commercial
    {
        #region Public Properties

        public string CommercialType { get; set; }

        public Demographic Demographic { get; set; }
        
        public int Id { get; set; }

        public string Title { get; set; }

        #endregion
    }
}