using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.Data.Factories.Contracts
{
    public interface ICommercialFactory
    {
        Task<List<Commercial>> GetCommercialsAsync();
    }
}
