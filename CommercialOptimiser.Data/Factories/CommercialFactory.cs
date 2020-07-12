using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;
using CommercialOptimiser.Data.Factories.Contracts;
using CommercialOptimiser.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace CommercialOptimiser.Data.Factories
{
    public class CommercialFactory : ICommercialFactory
    {
        #region Members

        private readonly DatabaseContext _databaseContext;
        private readonly ITableModelConverter _tableModelConverter;

        #endregion

        #region Constructors

        public CommercialFactory(
            DatabaseContext dbContext,
            ITableModelConverter tableModelConverter)
        {
            _databaseContext = dbContext;
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