using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialOptimiser.Data;
using CommercialOptimiser.Models;

namespace CommercialOptimiser.ViewModels
{
    public interface ICommercialsViewModel
    {
        #region Public Properties

        List<Break> Breaks { get; }

        #endregion

        #region Public Methods

        void AddCommercial(Break aBreak);

        void ApplyOptimalPlacement();

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

        private readonly Dictionary<Break, List<Commercial>> _breakCommercials =
            new Dictionary<Break, List<Commercial>>();

        #endregion

        #region Public Properties

        public List<Break> Breaks { get; private set; } = new List<Break>();

        #endregion

        #region Public Methods

        public void AddCommercial(Break aBreak)
        {
            _breakCommercials.TryGetValue(aBreak, out var commercials);
            if (commercials == null)
            {
                commercials = new List<Commercial>();
                _breakCommercials.Add(aBreak, commercials);
            }
        }

        public void ApplyOptimalPlacement()
        {
        }

        public int GetCommercialBreakRating(Break aBreak, Commercial commercial)
        {
            var matchingBreakDemographic =
                aBreak.BreakDemographics?.FirstOrDefault(
                    demo => demo.Demographic?.Name == commercial.Demographic?.Name);
            return matchingBreakDemographic?.Rating ?? 0;
        }

        public List<Commercial> GetCommercials(Break aBreak)
        {
            _breakCommercials.TryGetValue(aBreak, out var commercials);
            if (commercials == null)
                return new List<Commercial>();
            return commercials;
        }

        public async Task InitializeAsync()
        {
            var service = new CommercialService();
            Breaks = await service.GetBreaksAsync();

            _breakCommercials.Add(
                Breaks[0],
                new List<Commercial>
                {
                    new Commercial
                    {
                        Name = "ACommercial",
                        Type = CommercialType.Travel,
                        Demographic = new Demographic {Name = "W25-30"}
                    }
                });

            _breakCommercials.Add(
                Breaks[1],
                new List<Commercial>
                {
                    new Commercial {Name = "BCommercial"},
                    new Commercial {Name = "CCommercial"}
                });

            _breakCommercials.Add(
                Breaks[2],
                new List<Commercial>
                {
                    new Commercial {Name = "DCommercial"},
                    new Commercial {Name = "ECommercial"},
                    new Commercial {Name = "FCommercial"}
                });
        }

        public void RemoveCommercialFromBreak(Break aBreak, Commercial commercial)
        {
            _breakCommercials.TryGetValue(aBreak, out var commercials);
            commercials?.Remove(commercial);
        }

        public void Reset()
        {
            _breakCommercials.Clear();
        }

        #endregion
    }
}