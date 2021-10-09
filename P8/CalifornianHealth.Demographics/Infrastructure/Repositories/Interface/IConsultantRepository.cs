using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data;

namespace CalifornianHealth.Demographics.Infrastructure.Repositories.Interface
{
    public interface IConsultantRepository
    {
        Task<List<Consultant>> GetConsultant();
        Task<Consultant> GetConsultant(int id);
        Task UpdateConsultant(Consultant consultant);
        Task SaveConsultant(Consultant consultant);
        Task DeleteConsultant(Consultant consultant);
    }
}