using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;
using CommercialOptimiser.Data.Factories.Contracts;
using CommercialOptimiser.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace CommercialOptimiser.Data.Factories
{
    public class BreakFactory : IBreakFactory
    {
        #region Members

        private readonly DatabaseContext _databaseContext;
        private readonly ITableModelConverter _tableModelConverter;

        #endregion

        #region Constructors

        public BreakFactory(
            DatabaseContext dbContext,
            ITableModelConverter tableModelConverter)
        {
            _databaseContext = dbContext;
            _tableModelConverter = tableModelConverter;
        }

        #endregion

        #region Public Methods

        public async Task<List<Break>> GetBreaksAsync()
        {
            var breakTables =
                await _databaseContext.Breaks
                    .Include(aBreak => aBreak.BreakDemographics)
                    .ThenInclude(breakDemographic => breakDemographic.Demographic)
                    .ToListAsync();

            var breaks = breakTables.Select(_tableModelConverter.ConvertTableToModel);
            return breaks.ToList();
        }

        #endregion
    }
}