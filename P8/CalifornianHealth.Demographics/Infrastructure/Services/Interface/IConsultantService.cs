using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data.DTO;

namespace CalifornianHealth.Demographics.Infrastructure.Services.Interface
{
    public interface IConsultantService
    {
        Task<List<ConsultantDto>> GetConsultant();
        Task<ConsultantDto> GetConsultant(int id);
        Task UpdateConsultant(ConsultantDto consultant);
        Task SaveConsultant(ConsultantDto consultant);
        Task DeleteConsultant(int id);
    }
}