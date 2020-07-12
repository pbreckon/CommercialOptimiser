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
        private readonly IUserFactory _userFactory;

        #endregion

        #region Constructors

        public UserBreakCommercialService(
            IBreakFactory breakFactory,
            IUserFactory userFactory,
            IUserReportBreakFactory userReportBreakFactory,
            IOptimiserHelper optimiserHelper)
        {
            _breakFactory = breakFactory;
            _userFactory = userFactory;
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

            var user = await _userFactory.GetUserAsync(uniqueUserId);

            if (user == null)
            {
                user = new User { UniqueUserId = uniqueUserId};
                await _userFactory.AddUserAsync(user);
            }

            await _userReportBreakFactory.DeleteReportBreaksAsync(user.Id);

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
                        User = user
                    };

                await _userReportBreakFactory.AddReportBreakAsync(userReportBreak);
            }
        }

        public async Task<List<UserReportBreak>> GetUserReportBreaksAsync(string uniqueUserId)
        {
            var user = await _userFactory.GetUserAsync(uniqueUserId);
            if (user == null) return null;

            return await _userReportBreakFactory.GetReportBreaksAsync(user.Id); 
        }

        #endregion
    }
}