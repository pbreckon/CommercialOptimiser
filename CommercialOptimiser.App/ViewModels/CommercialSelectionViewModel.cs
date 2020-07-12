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
        bool PreventCancel { get; }
        bool PreventOptimise { get; }

        int SelectedCommercialCount { get; }

        #endregion

        #region Public Methods

        Task InitializeAsync();

        Task<bool> OptimiseCommercialAllocationAsync();

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

        public bool AllSelected
        {
            get { return Commercials.All(value => value.Checked); }
            set
            {
                if (value)
                {
                    foreach (var commercial in Commercials)
                        commercial.Checked = true;
                }
            }
        }

        public int BreakCapacity => _breaks?.Sum(value => value.Capacity) ?? 0;

        public List<CommercialViewModel> Commercials { get; set; }

        public bool PreventCancel => false;

        public bool PreventOptimise =>
            Commercials == null ||
            _breaks == null ||
            SelectedCommercialCount < BreakCapacity;

        public int SelectedCommercialCount => SelectedCommercials?.Count ?? 0;

        #endregion

        #region Public Methods

        public async Task InitializeAsync()
        {
            Commercials =
                (await _apiHelper.GetCommercialsAsync())?
                .OrderBy(value => value.Id)
                .Select(value => new CommercialViewModel(value)).ToList();

            _breaks = await _apiHelper.GetBreaksAsync();
        }

        public async Task<bool> OptimiseCommercialAllocationAsync()
        {
            if (BreakCapacity == 0) return false;
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