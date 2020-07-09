using CommercialOptimiser.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialOptimiser.Api.Services.Contracts
{
    public interface IBreakService
    {
        #region Public Methods

        Task<IEnumerable<Break>> GetBreaksAsync();

        Task<IEnumerable<BreakCommercials>> GetOptimalBreakCommercialsAsync();

        #endregion
    }
}