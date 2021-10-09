using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data;
using CalifornianHealth.Booking.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CalifornianHealth.Booking.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private static BookingContext _bookingContext;
        private static DbSet<Data.Booking> _bookings;

        public BookingRepository(BookingContext context)
        {
            _bookingContext = context;
            _bookings = _bookingContext.Bookings;
        }

        public async Task<List<Data.Booking>> GetBooking()
        {
            return await _bookings.ToListAsync();
        }

        public async Task<Data.Booking> GetBooking(int id)
        {
            return await _bookings.FindAsync(id);
        }

        public async Task<List<Data.Booking>> GetBookingByConsultantId(int consultantId)
        {
            return await _bookings.Where(x => x.ConsultantId == consultantId).ToListAsync();
        }

        public async Task UpdateBooking(Data.Booking booking)
        {
            _bookings.UpdateRange(booking);
            await SaveChangesAsync();
        }

        public async Task SaveBooking(Data.Booking booking)
        {
            await _bookings.AddAsync(booking);
            await SaveChangesAsync();
        }

        public async Task DeleteBooking(Data.Booking booking)
        {
            _bookings.RemoveRange(booking);
            await SaveChangesAsync();
        }

        private static async Task SaveChangesAsync()
        {
            await _bookingContext.SaveChangesAsync();
        }
    }
}