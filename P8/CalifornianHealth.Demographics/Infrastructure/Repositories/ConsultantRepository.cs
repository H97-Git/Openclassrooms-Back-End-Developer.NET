using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data;
using CalifornianHealth.Demographics.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CalifornianHealth.Demographics.Infrastructure.Repositories
{
    public class ConsultantRepository : IConsultantRepository
    {
        private static DemographicsContext _demographicsContext;
        private static DbSet<Consultant> _consultants;

        public ConsultantRepository(DemographicsContext context)
        {
            _demographicsContext = context;
            _consultants = _demographicsContext.Consultants;
        }

        public async Task<List<Consultant>> GetConsultant()
        {
            return await _consultants.ToListAsync();
        }

        public async Task<Consultant> GetConsultant(int id)
        {
            return await _consultants.FindAsync(id);
        }

        public async Task UpdateConsultant(Consultant consultant)
        {
            _consultants.UpdateRange(consultant);
            await SaveChangesAsync();
        }

        public async Task SaveConsultant(Consultant consultant)
        {
            await _consultants.AddAsync(consultant);
            await SaveChangesAsync();
        }

        public async Task DeleteConsultant(Consultant consultant)
        {
            _consultants.RemoveRange(consultant);
            await SaveChangesAsync();
        }

        private static async Task SaveChangesAsync()
        {
            await _demographicsContext.SaveChangesAsync();
        }
    }
}