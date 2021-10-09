using System;

namespace CalifornianHealth.Booking.Data
{
    public class Booking
    {
        public int Id { get; init; }

        public int ConsultantId { get; set; }
        public int PatientId { get; set; }
        public int AvailabilityId { get; set; }

        public DateTime Appointment { get; set; }
    }
}