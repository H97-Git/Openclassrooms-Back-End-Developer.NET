using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data;

namespace CalifornianHealth.Demographics.Infrastructure.Repositories.Interface
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetPatient();
        Task<Patient> GetPatient(int id);
        Task UpdatePatient(Patient patient);
        Task SavePatient(Patient patient);
        Task DeletePatient(Patient patient);
    }
}