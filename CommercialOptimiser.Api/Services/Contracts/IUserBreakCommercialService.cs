using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.Api.Services.Contracts
{
    public interface IUserBreakCommercialService
    {
        #region Public Methods

        Task CalculateOptimalBreakCommercialsAsync(
            string uniqueUserId,
            List<Commercial> commercials);

        Task<List<UserReportBreak>> GetUserReportBreaksAsync(string uniqueUserId);

        #endregion
    }
}