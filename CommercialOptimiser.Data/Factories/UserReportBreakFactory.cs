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

        public async Task DeleteReportBreaksAsync(int userId)
        {
            var currentUserBreaks =
                await _databaseContext.UserReportBreaks.Where(
                    value =>
                        value.User.Id == userId).ToListAsync();
            foreach (var currentUserBreak in currentUserBreaks)
            {
                var currentUserBreakCommercials =
                    await _databaseContext.UserReportBreakCommercials.Where(
                        value =>
                            value.UserReportBreak.Id == currentUserBreak.Id).ToListAsync();

                _databaseContext.UserReportBreakCommercials.RemoveRange(currentUserBreakCommercials);
                _databaseContext.SaveChanges();
            }

            _databaseContext.UserReportBreaks.RemoveRange(currentUserBreaks);
            _databaseContext.SaveChanges();
        }

        public async Task AddReportBreakAsync(UserReportBreak userReportBreak)
        {
            var userReportBreakTable = _tableModelConverter.ConvertModelToTable(userReportBreak);

            _databaseContext.DetachLocal(userReportBreakTable.User);
            _databaseContext.SaveChanges();

            await _databaseContext.UserReportBreaks.AddAsync(userReportBreakTable);
            _databaseContext.SaveChanges();
        }

        public async Task<List<UserReportBreak>> GetReportBreaksAsync(int userId)
        {
            var userReportBreakTables =
                await _databaseContext.UserReportBreaks
                    .Include(urb => urb.UserReportBreakCommercials)
                    .Include(urb => urb.User)
                    .Where(ubt => ubt.User.Id == userId)
                    .OrderBy(ubt => ubt.BreakTitle)
                    .ToListAsync();

            var userBreaks = userReportBreakTables.Select(_tableModelConverter.ConvertTableToModel);
            return userBreaks.ToList();
        }
    }
}
