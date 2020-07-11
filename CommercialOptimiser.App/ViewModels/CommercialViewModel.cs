using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.App.ViewModels
{
    public class CommercialViewModel
    {
        #region Constructors

        public CommercialViewModel(Commercial commercial)
        {
            Commercial = commercial;
        }

        #endregion

        #region Public Properties

        public bool Checked { get; set; }

        public Commercial Commercial { get; set; }

        #endregion
    }
}