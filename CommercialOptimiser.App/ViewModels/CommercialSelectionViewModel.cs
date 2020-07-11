using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialOptimiser.App.Helpers;
using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.App.ViewModels
{
    public interface ICommercialSelectionViewModel
    {
        #region Public Properties

        int BreakCapacity { get; }

        List<CommercialViewModel> Commercials { get; set; }

        #endregion

        #region Public Methods

        Task InitializeAsync();

        Task OptimiseCommercialAllocationAsync();

        #endregion
    }

    public class CommercialSelectionViewModel : ICommercialSelectionViewModel
    {
        #region Members

        private readonly IApiHelper _apiHelper;

        private List<Break> _breaks;

        #endregion

        #region Constructors

        public CommercialSelectionViewModel(
            IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        #endregion

        #region Public Properties

        public int BreakCapacity => _breaks?.Sum(value => value.Capacity) ?? 0;

        public List<CommercialViewModel> Commercials { get; set; }

        #endregion

        #region Public Methods

        public async Task InitializeAsync()
        {
            Commercials =
                (await _apiHelper.GetCommercialsAsync())?
                .OrderBy(value => value.Title)
                .Select(value => new CommercialViewModel(value)).ToList();

            _breaks = await _apiHelper.GetBreaksAsync();
        }

        public async Task OptimiseCommercialAllocationAsync()
        {
            if (BreakCapacity == 0) return;
            var selectedCommercials =
                Commercials.Where(value => value.Checked).Select(value => value.Commercial).ToList();
            if (selectedCommercials.Count < BreakCapacity) return;

            await _apiHelper.CalculateOptimalBreakCommercialsAsync(selectedCommercials);
        }

        #endregion
    }
}