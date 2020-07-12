using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;

namespace CommercialOptimiser.Data.Factories.Contracts
{
    public interface IBreakFactory
    {
        #region Public Methods

        Task<List<Break>> GetBreaksAsync();

        #endregion
    }
}