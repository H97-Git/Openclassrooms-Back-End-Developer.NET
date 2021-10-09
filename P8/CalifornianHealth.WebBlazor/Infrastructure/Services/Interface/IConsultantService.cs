using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.WebBlazor.Models;
using Microsoft.Extensions.Configuration;

namespace CalifornianHealth.WebBlazor.Infrastructure.Services.Interface
{
    public interface IConsultantService
    {
        public string ErrorMessage { get; set; }
        public IConfiguration Configuration { get; }
        Task<List<ConsultantModel>> Get();
    }
}