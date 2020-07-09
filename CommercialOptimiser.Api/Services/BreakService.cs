using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialOptimiser.Api.Database;
using CommercialOptimiser.Api.Database.Tables;

namespace CommercialOptimiser.Api.Services
{
    public class BreakService : IBreakService
    {
        #region Members

        private readonly DatabaseContext _databaseContext;
        private readonly ITableModelConverter _tableModelConverter;

        #endregion

        #region Constructors

        public BreakService(
            DatabaseContext databaseContext,
            ITableModelConverter tableModelConverter)
        {
            _databaseContext = databaseContext;
            _tableModelConverter = tableModelConverter;
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<Break>> GetBreaksAsync()
        {
            var breakTables =
                await _databaseContext.Breaks
                    .Include(aBreak => aBreak.BreakDemographics)
                    .ThenInclude(breakDemographic => breakDemographic.Demographic)
                    .ToListAsync();

            var breaks = breakTables.Select(_tableModelConverter.ConvertTableToModel);
            return breaks;
        }

        public async Task<IEnumerable<BreakCommercials>> GetOptimalBreakCommercialsAsync()
        {
            var breakTables =
                await _databaseContext.Breaks
                    .Include(aBreak => aBreak.BreakDemographics)
                    .ThenInclude(breakDemographic => breakDemographic.Demographic)
                    .ToListAsync();


            return null;
            //return new List<BreakCommercials> 
            //    {
            //        new BreakCommercials() 
            //        {
            //            Break = breaks.First(),
            //            Commercials = new List<Commercial> { new Commercial { Title = "Hoooo" } }
            //        }
            //    };
        }

        #endregion
    }
}