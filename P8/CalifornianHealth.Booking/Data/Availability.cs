using System;

namespace CalifornianHealth.Booking.Data
{
    public class Availability
    {
        public int Id { get; init; }
        public int ConsultantId { get; set; }
        public DateTime DateTime { get; set; }
    }
}