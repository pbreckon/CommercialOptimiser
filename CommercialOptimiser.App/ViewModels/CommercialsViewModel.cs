using CommercialOptimiser.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialOptimiser.App.Helpers;

namespace CommercialOptimiser.App.ViewModels
{
    public interface ICommercialsViewModel
    {
        #region Public Properties

        List<Break> Breaks { get; }

        #endregion

        #region Public Methods

        void AddCommercial(Break aBreak);

        Task ApplyOptimalPlacementAsync();

        int GetCommercialBreakRating(Break aBreak, Commercial commercial);

        List<Commercial> GetCommercials(Break aBreak);

        Task InitializeAsync();

        void RemoveCommercialFromBreak(Break aBreak, Commercial commercial);

        void Reset();

        #endregion
    }

    public class CommercialsViewModel : ICommercialsViewModel
    {
        #region Members

        private readonly Dictionary<Break, List<Commercial>> _allBreakCommercials =
            new Dictionary<Break, List<Commercial>>();

        private readonly IApiHelper _apiHelper;

        #endregion

        #region Constructors

        public CommercialsViewModel(
            IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        #endregion

        #region Public Properties

        public List<Break> Breaks { get; private set; } = new List<Break>();

        #endregion

        #region Public Methods

        public void AddCommercial(Break aBreak)
        {
            var commercials = GetCommercialsForBreak(aBreak);
        }

        public async Task ApplyOptimalPlacementAsync()
        {
            var breakCommercials = await GetOptimalBreakCommercialsAsync();

            _allBreakCommercials.Clear();

            foreach (var breakCommercial in breakCommercials)
            {
                var matchingBreak =
                    Breaks.FirstOrDefault(
                        aBreak => breakCommercial.Break.Id == aBreak.Id);

                if (matchingBreak != null)
                    _allBreakCommercials.Add(matchingBreak, breakCommercial.Commercials.ToList());
            }
        }

        public int GetCommercialBreakRating(Break aBreak, Commercial commercial)
        {
            var matchingBreakDemographic =
                aBreak.BreakDemographics?.FirstOrDefault(
                    demo => demo.Demographic?.Title == commercial.Demographic?.Title);
            return matchingBreakDemographic?.Rating ?? 0;
        }

        public List<Commercial> GetCommercials(Break aBreak)
        {
            var commercials = GetCommercialsForBreak(aBreak);
            return commercials;
        }

        public async Task InitializeAsync()
        {
            Breaks = await GetBreaksAsync();
        }

        public void RemoveCommercialFromBreak(Break aBreak, Commercial commercial)
        {
            var commercials = GetCommercialsForBreak(aBreak);
            commercials.Remove(commercial);
        }

        public void Reset()
        {
            _allBreakCommercials.Clear();
        }

        #endregion

        #region Private Methods

        private async Task<List<Break>> GetBreaksAsync()
        {
            var breaks = await _apiHelper.GetBreaksAsync();
            return breaks.ToList();
        }

        private List<Commercial> GetCommercialsForBreak(Break aBreak)
        {
            if (!_allBreakCommercials.TryGetValue(aBreak, out var commercials))
            {
                commercials = new List<Commercial>();
                _allBreakCommercials.Add(aBreak, commercials);
            }

            return commercials;
        }

        private async Task<List<BreakCommercials>> GetOptimalBreakCommercialsAsync()
        {
            var breakCommercials = await _apiHelper.GetOptimalBreakCommercialsAsync();
            return breakCommercials.ToList();
        }

        #endregion
    }
}