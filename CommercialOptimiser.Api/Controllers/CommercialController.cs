using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialOptimiser.Api.Controllers
{
    [ApiController]
    [Route("Commercials")]
    public class CommercialController : ControllerBase
    {
        #region Members

        private readonly ICommercialService _commercialService;

        #endregion

        #region Constructors

        public CommercialController(
            ICommercialService service)
        {
            _commercialService = service;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the available commercial breaks.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Commercial>> Get()
        {
            var data = await _commercialService.GetCommercialsAsync();
            return data;
        }

        #endregion
    }
}