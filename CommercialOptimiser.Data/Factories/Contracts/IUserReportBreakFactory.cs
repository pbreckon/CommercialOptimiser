using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.Data.Factories.Contracts
{
    public interface IUserReportBreakFactory
    {
        #region Public Methods

        Task AddReportBreaksAsync(string uniqueUserId, List<UserReportBreak> userReportBreaks);
        Task<List<UserReportBreak>> GetReportBreaksAsync(string uniqueUserId);

        #endregion
    }
}