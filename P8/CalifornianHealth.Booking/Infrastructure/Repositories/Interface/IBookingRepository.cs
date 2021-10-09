using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalifornianHealth.Booking.Infrastructure.Repositories.Interface
{
    public interface IBookingRepository
    {
        Task<List<Data.Booking>> GetBooking();
        Task<Data.Booking> GetBooking(int id);
        Task<List<Data.Booking>> GetBookingByConsultantId(int id);
        Task UpdateBooking(Data.Booking booking);
        Task SaveBooking(Data.Booking booking);
        Task DeleteBooking(Data.Booking booking);
    }
}