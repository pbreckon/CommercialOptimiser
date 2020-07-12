using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommercialOptimiser.Data.Factories.Contracts;

namespace CommercialOptimiser.Api.Services
{
    public class BreakService : IBreakService
    {
        #region Members

        private readonly IBreakFactory _breakFactory;

        #endregion

        #region Constructors

        public BreakService(IBreakFactory breakFactory)
        {
            _breakFactory = breakFactory;
        }

        #endregion

        #region Public Methods

        public async Task<List<Break>> GetBreaksAsync()
        {
            return await _breakFactory.GetBreaksAsync();
        }

        #endregion
    }
}