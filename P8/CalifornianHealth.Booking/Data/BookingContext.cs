using Microsoft.EntityFrameworkCore;

namespace CalifornianHealth.Booking.Data
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
    }
}