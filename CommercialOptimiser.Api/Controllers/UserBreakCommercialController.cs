using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CommercialOptimiser.Api.Controllers
{
    [ApiController]
    [Route("userBreaks")]
    public class UserBreakCommercialController : ControllerBase
    {
        #region Members
        
        private readonly IUserBreakCommercialService _userBreakCommercialService;

        #endregion

        #region Constructors

        public UserBreakCommercialController(
            IUserBreakCommercialService service)
        {
            _userBreakCommercialService = service;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the available commercial breaks.
        /// </summary>
        [HttpGet]
        [Route("{uniqueUserId}")]
        public async Task<List<UserReportBreak>> GetUserReportBreaksAsync(string uniqueUserId)
        {
            var data =
                await _userBreakCommercialService.GetUserReportBreaksAsync(uniqueUserId);
            return data;
        }

        /// <summary>
        /// Calculates and stores the available commercial breaks.
        /// </summary>
        [HttpPost]
        [Route("{uniqueUserId}")]
        public async Task CalculateOptimalBreakCommercialsAsync(
            string uniqueUserId,
            List<Commercial> commercials)
        {
            await _userBreakCommercialService.CalculateOptimalBreakCommercialsAsync(
                uniqueUserId,
                commercials);
        }

        #endregion
    }
}