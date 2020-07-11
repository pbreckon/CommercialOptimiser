using CommercialOptimiser.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using CommercialOptimiser.App.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace CommercialOptimiser.App.ViewModels
{
    public interface IAllocationReportViewModel
    {
        #region Public Properties

        List<UserReportBreak> UserReportBreaks { get; }

        int UserReportBreaksTotal { get; }

        #endregion

        #region Public Methods

        Task InitializeAsync();

        #endregion
    }

    public class AllocationReportViewModel : IAllocationReportViewModel
    {
        #region Members

        private readonly IApiHelper _apiHelper;

        #endregion

        #region Constructors

        public AllocationReportViewModel(
            IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        #endregion

        #region Public Properties

        public List<UserReportBreak> UserReportBreaks { get; private set; }

        public int UserReportBreaksTotal =>
            UserReportBreaks?.Sum(value => value.UserReportBreakCommercials.Sum(commercial => commercial.Rating)) ?? 0;

        #endregion

        #region Public Methods

        public async Task InitializeAsync()
        {
            UserReportBreaks = await _apiHelper.GetUserReportBreaksAsync();
        }

        #endregion
    }
}