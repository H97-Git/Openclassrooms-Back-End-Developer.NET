using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    public class BookingService : IBookingService
    {
        private readonly HttpClient _client;

        public BookingService(IHttpClientFactory clientFactory, IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _client = clientFactory.CreateClient();
        }

        public async Task<List<BookingModel>> Get()
        {
            try
            {
                var apiResponse =
                    await _client.GetAsync(Resources.californianhealth_gateway_Booking);
                if (!apiResponse.IsSuccessStatusCode)
                    return new List<BookingModel>();

                var content = await apiResponse.Content.ReadAsStringAsync();
                var bookingModels = JsonConvert.DeserializeObject<List<BookingModel>>(content);

                return bookingModels;
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);

                return new List<BookingModel>();
            }
        }

        public string ErrorMessage { get; set; }
        public IConfiguration Configuration { get; }

        // Deprecated ? 
        public async Task Save(BookingModel booking)
        {
            throw new NotImplementedException();
            var addContent = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8);
            addContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var apiResponse = await _client.PostAsync("https://localhost:6001/Booking", addContent);
                if (apiResponse.IsSuccessStatusCode)
                {
                    var content = await apiResponse.Content.ReadAsStringAsync();
                    ErrorMessage = string.Empty;
                }
            }
            catch (HttpRequestException exception)
            {
                HandleHttpRequestException(exception);
            }
        }

        private void HandleHttpRequestException(HttpRequestException ex)
        {
            ErrorMessage = "Api can't be reached.";
            Log.Error("Api can't be reached : {message}", ex.Message);
        }

        private record Command(BookingModel Booking);
    }
}