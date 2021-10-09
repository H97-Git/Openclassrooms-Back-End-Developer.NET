using System;

namespace CalifornianHealth.Booking.Data.DTO
{
    public class BookingDto
    {
        public int Id { get; init; }

        public int ConsultantId { get; init; }
        public int PatientId { get; set; }
        public int AvailabilityId { get; set; }

        public DateTime Appointment { get; set; }
    }
}