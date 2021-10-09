using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;

namespace CalifornianHealth.Booking.Infrastructure.Services.Interface
{
    public interface IAvailabilityService
    {
        Task<List<AvailabilityDto>> GetAvailabilities();
        Task<AvailabilityDto> GetAvailability(int id);
        Task<List<AvailabilityDto>> FilterByConsultantId(int consultantId);
        Task UpdateAvailability(AvailabilityDto availabilityDto);
        Task SaveAvailability(AvailabilityDto availabilityDto);
        Task DeleteAvailability(int id);
    }
}