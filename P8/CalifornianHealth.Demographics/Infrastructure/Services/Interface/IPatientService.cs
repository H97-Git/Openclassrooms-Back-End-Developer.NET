using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data.DTO;

namespace CalifornianHealth.Demographics.Infrastructure.Services.Interface
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetPatient();
        Task<PatientDto> GetPatient(int id);
        Task UpdatePatient(PatientDto patientDto);
        Task SavePatient(PatientDto patientDto);
        Task DeletePatient(int id);
    }
}