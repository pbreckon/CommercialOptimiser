using System;
using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using CommercialOptimiser.Api.Database;
using CommercialOptimiser.Api.Database.Tables;
using CommercialOptimiser.Api.Helpers;

namespace CommercialOptimiser.Api.Services
{
    public class UserBreakCommercialService : IUserBreakCommercialService
    {
        #region Members

        private readonly DatabaseContext _databaseContext;
        private readonly IOptimiserHelper _optimiserHelper;
        private readonly ITableModelConverter _tableModelConverter;

        #endregion

        #region Constructors

        public UserBreakCommercialService(
            DatabaseContext databaseContext,
            ITableModelConverter tableModelConverter,
            IOptimiserHelper optimiserHelper)
        {
            _databaseContext = databaseContext;
            _tableModelConverter = tableModelConverter;
            _optimiserHelper = optimiserHelper;
        }

        #endregion

        #region Public Methods

        public async Task CalculateOptimalBreakCommercialsAsync(
            string uniqueUserId,
            List<Commercial> commercials)
        {
            var breaks = await _databaseContext.Breaks.ToListAsync();

            var optimalBreakCommercials =
                _optimiserHelper.GetOptimalBreakCommercials(
                    breaks.Select(_tableModelConverter.ConvertTableToModel).ToList(),
                    commercials);

            var userTable = await _databaseContext.Users.FirstOrDefaultAsync(
                value =>
                    value.UserUniqueId == uniqueUserId);
            if (userTable == null)
            {
                userTable = new UserTable {UserUniqueId = uniqueUserId};
                await _databaseContext.Users.AddAsync(userTable);
                _databaseContext.SaveChanges();
            }

            var currentUserBreaks =
                await _databaseContext.UserReportBreaks.Where(
                    value =>
                        value.User.Id == userTable.Id).ToListAsync();
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

            //create report
            foreach (var breakCommercials in optimalBreakCommercials)
            {
                var userReportBreak =
                    new UserReportBreak
                    {
                        BreakTitle = breakCommercials.Break.Title,
                        UserReportBreakCommercials =
                            breakCommercials.Commercials.Select(
                                commercial =>
                                    new UserReportBreakCommercial
                                    {
                                        CommercialTitle = commercial.Title,
                                        Rating = breakCommercials.Break.BreakDemographics.FirstOrDefault(
                                                         value => value.Demographic.Id == commercial.Demographic.Id)
                                                     ?.Rating ?? 0
                                    }).ToList(),
                        User = _tableModelConverter.ConvertTableToModel(userTable)
                    };

                var userReportBreakTable = _tableModelConverter.ConvertModelToTable(userReportBreak);

                _databaseContext.DetachLocal(userReportBreakTable.User);
                _databaseContext.SaveChanges();

                await _databaseContext.UserReportBreaks.AddAsync(userReportBreakTable);
                _databaseContext.SaveChanges();
            }
        }

        public async Task<List<UserReportBreak>> GetUserReportBreaksAsync(string uniqueUserId)
        {
            var userReportBreakTables =
                await _databaseContext.UserReportBreaks
                    .Include(urb => urb.UserReportBreakCommercials)
                    .Include(urb => urb.User)
                    .Where(ubt => ubt.User.UserUniqueId == uniqueUserId)
                    .ToListAsync();

            var userBreaks = userReportBreakTables.Select(_tableModelConverter.ConvertTableToModel);
            return userBreaks.ToList();
        }

        #endregion
    }
}