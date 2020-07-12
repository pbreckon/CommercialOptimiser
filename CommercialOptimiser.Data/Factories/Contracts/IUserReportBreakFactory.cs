using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.Data.Factories.Contracts
{
    public interface IUserReportBreakFactory
    {
        #region Public Methods

        Task AddReportBreakAsync(UserReportBreak userReportBreak);
        Task DeleteReportBreaksAsync(int userId);
        Task<List<UserReportBreak>> GetReportBreaksAsync(int userId);

        #endregion
    }
}