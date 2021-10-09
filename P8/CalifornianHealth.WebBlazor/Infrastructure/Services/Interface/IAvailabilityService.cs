using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.WebBlazor.Models;
using Microsoft.Extensions.Configuration;

namespace CalifornianHealth.WebBlazor.Infrastructure.Services.Interface
{
    public interface IAvailabilityService
    {
        public string ErrorMessage { get; set; }
        public IConfiguration Configuration { get; }
        Task<List<AvailabilityModel>> Get();
        Task<List<AvailabilityModel>> GetByConsultantId(int consultantId);
    }
}