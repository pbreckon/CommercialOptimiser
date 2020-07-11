using CommercialOptimiser.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialOptimiser.Api.Services.Contracts
{
    public interface IBreakService
    {
        #region Public Methods

        Task<List<Break>> GetBreaksAsync();

        #endregion
    }
}