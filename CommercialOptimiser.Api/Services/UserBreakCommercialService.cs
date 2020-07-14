using System;
using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialOptimiser.Api.Helpers;
using CommercialOptimiser.Data.Factories.Contracts;

namespace CommercialOptimiser.Api.Services
{
    public class UserBreakCommercialService : IUserBreakCommercialService
    {
        #region Members

        private readonly IUserReportBreakFactory _userReportBreakFactory;
        private readonly IOptimiserHelper _optimiserHelper;
        private readonly IBreakFactory _breakFactory;

        #endregion

        #region Constructors

        public UserBreakCommercialService(
            IBreakFactory breakFactory,
            IUserReportBreakFactory userReportBreakFactory,
            IOptimiserHelper optimiserHelper)
        {
            _breakFactory = breakFactory;
            _userReportBreakFactory = userReportBreakFactory;
            _optimiserHelper = optimiserHelper;
        }

        #endregion

        #region Public Methods

        public async Task CalculateOptimalBreakCommercialsAsync(
            string uniqueUserId,
            List<Commercial> commercials)
        {
            var breaks = await _breakFactory.GetBreaksAsync();

            var optimalBreakCommercials =
                _optimiserHelper.GetOptimalBreakCommercials(
                    breaks,
                    commercials);
            
            //create report
            var userReportBreaks = new List<UserReportBreak>();
            foreach (var breakCommercials in optimalBreakCommercials)
            {
                userReportBreaks.Add(
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
                        UserUniqueId = uniqueUserId
                    });
            }

            await _userReportBreakFactory.AddReportBreaksAsync(uniqueUserId, userReportBreaks);
        }

        public async Task<List<UserReportBreak>> GetUserReportBreaksAsync(string uniqueUserId)
        {
            return await _userReportBreakFactory.GetReportBreaksAsync(uniqueUserId); 
        }

        #endregion
    }
}