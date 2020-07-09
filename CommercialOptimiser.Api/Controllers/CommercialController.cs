﻿using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessDayCalculator.Controllers
{
    [ApiController]
    [Route("Commercials")]
    public class CommercialController : ControllerBase
    {
        #region Members

        private readonly ILogger<CommercialController> _logger;
        private readonly ICommercialService _commercialService;

        #endregion

        #region Constructors

        public CommercialController(
            ILogger<CommercialController> logger,
            ICommercialService service)
        {
            _logger = logger;
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