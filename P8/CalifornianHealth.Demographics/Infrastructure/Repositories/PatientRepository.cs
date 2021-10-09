using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data;
using CalifornianHealth.Demographics.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CalifornianHealth.Demographics.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private static DemographicsContext _demographicsContext;
        private static DbSet<Patient> _patients;

        public PatientRepository(DemographicsContext context)
        {
            _demographicsContext = context;
            _patients = _demographicsContext.Patients;
        }

        public async Task<List<Patient>> GetPatient()
        {
            return await _patients.ToListAsync();
        }

        public async Task<Patient> GetPatient(int id)
        {
            return await _patients.FindAsync(id);
        }

        public async Task UpdatePatient(Patient patient)
        {
            _patients.UpdateRange(patient);
            await SaveChangesAsync();
        }

        public async Task SavePatient(Patient patient)
        {
            await _patients.AddAsync(patient);
            await SaveChangesAsync();
        }

        public async Task DeletePatient(Patient patient)
        {
            _patients.RemoveRange(patient);
            await SaveChangesAsync();
        }

        private static async Task SaveChangesAsync()
        {
            await _demographicsContext.SaveChangesAsync();
        }
    }
}