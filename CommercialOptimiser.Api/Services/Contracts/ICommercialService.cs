using CommercialOptimiser.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialOptimiser.Api.Services.Contracts
{
    public interface ICommercialService
    {
        Task<IEnumerable<Commercial>> GetCommercialsAsync();
    }
}
