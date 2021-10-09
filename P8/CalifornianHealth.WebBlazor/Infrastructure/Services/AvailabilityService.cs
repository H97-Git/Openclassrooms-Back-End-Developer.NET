using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CalifornianHealth.WebBlazor.Infrastructure.Services.Interface;
using CalifornianHealth.WebBlazor.Models;
using CalifornianHealth.WebBlazor.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;

namespace CalifornianHealth.WebBlazor.Infrastructure.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly HttpClient _client;

        public AvailabilityService(IHttpClientFactory clientFactory, IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _client = clientFactory.CreateClient();
        }

        public async Task<List<AvailabilityModel>> Get()
        {
            try
            {
                var apiResponse =
                    await _client.GetAsync(Resources.californianhealth_gateway_Availability);
                if (!apiResponse.IsSuccessStatusCode)
                    return new List<AvailabilityModel>();

                var content = await apiResponse.Content.ReadAsStringAsync();
                var availabilityModels = JsonConvert.DeserializeObject<List<AvailabilityModel>>(content);

                return availabilityModels;
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);

                return new List<AvailabilityModel>();
            }
        }

        public async Task<List<AvailabilityModel>> GetByConsultantId(int consultantId)
        {
            try
            {
                var apiResponse =
                    await _client.GetAsync(
                        Resources.californianhealth_gateway_Availability_By_Consultant + consultantId);
                if (!apiResponse.IsSuccessStatusCode)
                    return new List<AvailabilityModel>();

                var content = await apiResponse.Content.ReadAsStringAsync();
                var availabilityModels = JsonConvert.DeserializeObject<List<AvailabilityModel>>(content);

                return availabilityModels;
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);

                return new List<AvailabilityModel>();
            }
        }

        public string ErrorMessage { get; set; }
        public IConfiguration Configuration { get; }

        private void HandleHttpRequestException(HttpRequestException ex)
        {
            ErrorMessage = "Api can't be reached.";
            Log.Error("Api can't be reached : {message}", ex.Message);
        }

        private record Command(AvailabilityModel Availability);
    }
}