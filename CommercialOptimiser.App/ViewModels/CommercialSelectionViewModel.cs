using System;
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

        bool AllSelected { get; set; }

        int BreakCapacity { get; }

        List<CommercialViewModel> Commercials { get; set; }

        bool IsBusy { get; set; }

        bool PreventCancel { get; }

        bool PreventOptimise { get; }

        int SelectedCommercialCount { get; }

        bool SufficientCommercialsSelected { get; }

        #endregion

        #region Public Methods

        Task InitializeAsync(bool alwaysPreventCancel);

        Task<bool> OptimiseCommercialAllocationAsync();

        #endregion
    }

    public class CommercialSelectionViewModel : ICommercialSelectionViewModel
    {
        #region Members

        private readonly IApiHelper _apiHelper;

        private List<Break> _breaks;

        private bool _alwaysPreventCancel;

        #endregion

        #region Constructors

        public CommercialSelectionViewModel(
            IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        #endregion

        #region Public Properties

        public bool AllSelected
        {
            get { return Commercials?.All(value => value.Checked) ?? false; }
            set
            {
                if (value && Commercials != null)
                {
                    foreach (var commercial in Commercials)
                        commercial.Checked = true;
                }
            }
        }

        public int BreakCapacity => _breaks?.Sum(value => value.Capacity) ?? 0;

        public List<CommercialViewModel> Commercials { get; set; }

        public bool IsBusy { get; set; }

        public bool PreventCancel => _alwaysPreventCancel || IsBusy;

        public bool PreventOptimise =>
            SufficientCommercialsSelected ||
            IsBusy;

        public int SelectedCommercialCount => SelectedCommercials?.Count ?? 0;

        public bool SufficientCommercialsSelected =>
            Commercials == null ||
            _breaks == null ||
            SelectedCommercialCount < BreakCapacity;

        #endregion

        #region Public Methods

        public async Task InitializeAsync(bool preventCancel)
        {
            _alwaysPreventCancel = preventCancel;

            Commercials =
                (await _apiHelper.GetCommercialsAsync())?
                .OrderBy(value => value.Id)
                .Select(value => new CommercialViewModel(value)).ToList();

            _breaks = await _apiHelper.GetBreaksAsync();
        }

        public async Task<bool> OptimiseCommercialAllocationAsync()
        {
            if (BreakCapacity == 0) return false;
            var selectedCommercialViewModels = SelectedCommercials;
            if (selectedCommercialViewModels == null) return false;

            var selectedCommercials =
                SelectedCommercials.Select(value => value.Commercial).ToList();
            if (selectedCommercials.Count < BreakCapacity) return false;

            await _apiHelper.CalculateOptimalBreakCommercialsAsync(selectedCommercials);
            return true;
        }

        #endregion

        #region Private Properties

        private List<CommercialViewModel> SelectedCommercials =>
            Commercials?.Where(value => value.Checked).ToList();

        #endregion
    }
}