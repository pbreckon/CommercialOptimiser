using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommercialOptimiser.Core.Models;
using CommercialOptimiser.Core.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CommercialOptimiser.App.Helpers
{
    public interface IApiHelper
    {
        #region Public Methods

        Task CalculateOptimalBreakCommercialsAsync(List<Commercial> commercials);

        Task<List<Break>> GetBreaksAsync();

        Task<List<Commercial>> GetCommercialsAsync();

        Task<List<UserReportBreak>> GetUserReportBreaksAsync();

        #endregion
    }

    public class ApiHelper : IApiHelper
    {
        #region Members

        private readonly HttpClient _client;
        private readonly ILogger<ApiHelper> _logger;

        #endregion

        #region Constructors

        public ApiHelper(
            IConfiguration configuration,
            ILogger<ApiHelper> logger)
        {
            var baseUrl = configuration["ApiBaseUrl"];
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);

            _logger = logger;
        }

        #endregion

        #region Public Methods

        public async Task CalculateOptimalBreakCommercialsAsync(
            List<Commercial> commercials)
        {
            var request = 
                new CalculateOptimalBreakCommercialsRequest
                {
                    Commercials = commercials,
                    UserUniqueId = GetUniqueUserId
                };
            var requestJson = JsonConvert.SerializeObject(commercials);

            var httpContent =
                new StringContent(
                    requestJson,
                    Encoding.UTF8,
                    "application/json");

            try
            {
                var response = 
                    await _client.PostAsync($"userBreaks/{GetUniqueUserId}", httpContent).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Error calling break commercials optimisation endpoint");
                throw;
            }
        }

        public async Task<List<Break>> GetBreaksAsync()
        {
            try
            {
                var response = await _client.GetAsync("breaks");
                var breaks = JsonConvert.DeserializeObject<Break[]>(
                    response.Content.ReadAsStringAsync().Result);
                return breaks.ToList();
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Error retrieving Breaks");
                throw;
            }
        }

        public async Task<List<Commercial>> GetCommercialsAsync()
        {
            try
            {
                var response = await _client.GetAsync("commercials");
                var commercials = JsonConvert.DeserializeObject<Commercial[]>(
                    response.Content.ReadAsStringAsync().Result);
                return commercials.ToList();
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Error retrieving Commercials");
                throw;
            }
        }

        public async Task<List<UserReportBreak>> GetUserReportBreaksAsync()
        {
            try
            {
                var response = await _client.GetAsync($"userBreaks/{GetUniqueUserId}");
                var userReportBreaks = JsonConvert.DeserializeObject<UserReportBreak[]>(
                    response.Content.ReadAsStringAsync().Result);
                return userReportBreaks.ToList();
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Error retrieving User Report Breaks");
                throw;
            }
        }

        #endregion

        #region Private Properties

        private string GetUniqueUserId => BlazorAppContext.UserIpAddress;

        #endregion
    }
}