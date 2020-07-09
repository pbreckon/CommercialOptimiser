using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CommercialOptimiser.Data.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CommercialOptimiser.App.Helpers
{
    public interface IApiHelper
    {
        #region Public Methods

        Task<IEnumerable<Break>> GetBreaksAsync();

        Task<IEnumerable<BreakCommercials>> GetOptimalBreakCommercialsAsync();

        #endregion
    }

    public class ApiHelper : IApiHelper
    {
        #region Members

        private readonly HttpClient _client;

        #endregion

        #region Constructors

        public ApiHelper(IConfiguration configuration)
        {
            var baseUrl = configuration["ApiHostName"];
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<Break>> GetBreaksAsync()
        {
            try
            {
                var response = await _client.GetAsync("breaks");
                var breaks = JsonConvert.DeserializeObject<Break[]>(
                    response.Content.ReadAsStringAsync().Result);
                return breaks;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BreakCommercials>> GetOptimalBreakCommercialsAsync()
        {
            try
            {
                var response = await _client.GetAsync("breaks/optimal");
                var breaks = JsonConvert.DeserializeObject<BreakCommercials[]>(
                    response.Content.ReadAsStringAsync().Result);
                return breaks;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        #endregion
    }
}