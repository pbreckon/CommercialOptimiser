using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommercialOptimiser.Data.Factories.Contracts;

namespace CommercialOptimiser.Api.Services
{
    public class CommercialService : ICommercialService
    {
        #region Members

        private readonly ICommercialFactory _commercialFactory;

        #endregion

        #region Constructors

        public CommercialService(
            ICommercialFactory commercialFactory)
        {
            _commercialFactory = commercialFactory;
        }

        #endregion

        #region Public Methods

        public async Task<List<Commercial>> GetCommercialsAsync()
        {
            return await _commercialFactory.GetCommercialsAsync();
        }

        #endregion
    }
}
