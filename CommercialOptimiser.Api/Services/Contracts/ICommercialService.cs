using CommercialOptimiser.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialOptimiser.Api.Services.Contracts
{
    public interface ICommercialService
    {
        Task<List<Commercial>> GetCommercialsAsync();
    }
}
