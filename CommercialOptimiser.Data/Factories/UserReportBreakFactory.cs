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
    public class UserReportBreakFactory : IUserReportBreakFactory
    {
        #region Members

        private readonly DatabaseContext _databaseContext;
        private readonly ITableModelConverter _tableModelConverter;

        #endregion

        #region Constructors

        public UserReportBreakFactory(
            DatabaseContext dbContext,
            ITableModelConverter tableModelConverter)
        {
            _databaseContext = dbContext;
            _tableModelConverter = tableModelConverter;
        }

        #endregion

        public async Task AddReportBreaksAsync(string uniqueUserId, List<UserReportBreak> userReportBreaks)
        {
            var table = _tableModelConverter.ConvertModelToTable(uniqueUserId, userReportBreaks);
            
            if (_databaseContext.UserReportBreaks.Any(e => e.UserUniqueId == uniqueUserId))
            {
                _databaseContext.Update(table);
            }
            else
            {
                await _databaseContext.AddAsync(table);
            }

            _databaseContext.SaveChanges();
        }

        public async Task<List<UserReportBreak>> GetReportBreaksAsync(string uniqueUserId)
        {
            var userReportBreakTable =
                await _databaseContext.UserReportBreaks
                    .Where(ubt => ubt.UserUniqueId == uniqueUserId).FirstOrDefaultAsync();
            if (userReportBreakTable == null) return null;

            return _tableModelConverter.ConvertTableToModel(userReportBreakTable);
        }
    }
}
