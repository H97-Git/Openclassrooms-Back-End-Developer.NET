using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data;
using CalifornianHealth.Booking.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CalifornianHealth.Booking.Infrastructure.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private static BookingContext _bookingContext;
        private static DbSet<Availability> _availabilities;

        public AvailabilityRepository(BookingContext context)
        {
            _bookingContext = context;
            _availabilities = _bookingContext.Availabilities;
        }

        public async Task<List<Availability>> GetAvailabilities()
        {
            return await _availabilities.ToListAsync();
        }

        public async Task<Availability> GetAvailability(int id)
        {
            return await _availabilities.FindAsync(id);
        }

        public async Task<List<Availability>> FilterByConsultantId(int consultantId)
        {
            return await _availabilities.Where(a => a.ConsultantId == consultantId).ToListAsync();
        }

        public async Task UpdateAvailability(Availability availability)
        {
            _availabilities.UpdateRange(availability);
            await SaveChangesAsync();
        }

        public async Task SaveAvailability(Availability availability)
        {
            await _availabilities.AddAsync(availability);
            await SaveChangesAsync();
        }

        public async Task DeleteAvailability(Availability availability)
        {
            _availabilities.RemoveRange(availability);
            await SaveChangesAsync();
        }

        private static async Task SaveChangesAsync()
        {
            await _bookingContext.SaveChangesAsync();
        }
    }
}