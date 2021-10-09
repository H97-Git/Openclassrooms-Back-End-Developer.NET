using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;

namespace CalifornianHealth.Booking.Infrastructure.Services.Interface
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetBooking();
        Task<BookingDto> GetBooking(int id);
        Task<List<BookingDto>> GetBookingByConsultantId(int consultantId);
        Task UpdateBooking(BookingDto availability);
        Task SaveBooking(BookingDto availability);
        Task DeleteBooking(int id);
    }
}