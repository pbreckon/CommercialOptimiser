using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessDayCalculator.Api.Controllers
{
    [ApiController]
    [Route("Breaks")]
    public class BreakController : ControllerBase
    {
        #region Members

        private readonly IBreakService _breakService;

        private readonly ILogger<BreakController> _logger;

        #endregion

        #region Constructors

        public BreakController(
            ILogger<BreakController> logger,
            IBreakService service)
        {
            _logger = logger;
            _breakService = service;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the available commercial breaks.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Break>> Get()
        {
            var data = await _breakService.GetBreaksAsync();
            return data;
        }

        #endregion
    }
}