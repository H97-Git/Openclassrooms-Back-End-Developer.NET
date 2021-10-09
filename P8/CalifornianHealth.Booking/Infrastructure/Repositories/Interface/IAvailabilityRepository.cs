using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data;

namespace CalifornianHealth.Booking.Infrastructure.Repositories.Interface
{
    public interface IAvailabilityRepository
    {
        Task<List<Availability>> GetAvailabilities();
        Task<Availability> GetAvailability(int id);
        Task<List<Availability>> FilterByConsultantId(int consultantId);
        Task UpdateAvailability(Availability booking);
        Task SaveAvailability(Availability booking);
        Task DeleteAvailability(Availability booking);
    }
}