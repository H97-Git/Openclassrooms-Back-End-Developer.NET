using System;

namespace CalifornianHealth.Booking.Data.DTO
{
    public class AvailabilityDto
    {
        public int Id { get; init; }
        public int ConsultantId { get; init; }
        public DateTime DateTime { get; set; }
    }
}