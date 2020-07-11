using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialOptimiser.Api.Database;
using CommercialOptimiser.Api.Database.Tables;

namespace CommercialOptimiser.Api.Services
{
    public class CommercialService : ICommercialService
    {
        #region Members

        private readonly DatabaseContext _databaseContext;
        private readonly ITableModelConverter _tableModelConverter;

        #endregion

        #region Constructors

        public CommercialService(
            DatabaseContext databaseContext,
            ITableModelConverter tableModelConverter)
        {
            _databaseContext = databaseContext;
            _tableModelConverter = tableModelConverter;
        }

        #endregion

        #region Public Methods

        public async Task<List<Commercial>> GetCommercialsAsync()
        {
            var commercialTables =
                await _databaseContext.Commercials
                    .Include(commercial => commercial.Demographic)
                    .ToListAsync();

            var commercials = commercialTables.Select(_tableModelConverter.ConvertTableToModel);
            return commercials.ToList();
        }

        #endregion
    }
}
