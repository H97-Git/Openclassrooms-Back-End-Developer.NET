using System;

namespace CalifornianHealth.WebBlazor.Models
{
    public class BookingModel
    {
        public int Id { get; set; }

        public int ConsultantId { get; set; }
        public int PatientId { get; set; }
        public int AvailabilityId { get; set; }

        public DateTime Appointment { get; set; }
    }
}