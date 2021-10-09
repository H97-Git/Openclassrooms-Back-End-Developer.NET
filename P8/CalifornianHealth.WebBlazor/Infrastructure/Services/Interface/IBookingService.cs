using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.WebBlazor.Models;
using Microsoft.Extensions.Configuration;

namespace CalifornianHealth.WebBlazor.Infrastructure.Services.Interface
{
    public interface IBookingService
    {
        public string ErrorMessage { get; set; }
        public IConfiguration Configuration { get; }
        Task<List<BookingModel>> Get();
    }
}