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
    public class ConsultantService : IConsultantService
    {
        private readonly HttpClient _client;

        public ConsultantService(IHttpClientFactory clientFactory, IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _client = clientFactory.CreateClient();
        }

        public IConfiguration Configuration { get; }
        public string ErrorMessage { get; set; }

        public async Task<List<ConsultantModel>> Get()
        {
            try
            {
                var apiResponse =
                    await _client.GetAsync(Resources.californianhealth_gateway_Consultant);
                if (!apiResponse.IsSuccessStatusCode)
                    return new List<ConsultantModel>();

                var content = await apiResponse.Content.ReadAsStringAsync();
                var consultantModels = JsonConvert.DeserializeObject<List<ConsultantModel>>(content);

                return consultantModels;
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);

                return new List<ConsultantModel>();
            }
        }

        private void HandleHttpRequestException(HttpRequestException ex)
        {
            ErrorMessage = "Api can't be reached.";
            Log.Error("Api can't be reached : {message}", ex.Message);
        }

        private record Command(ConsultantModel Consultant);
    }
}