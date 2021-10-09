using System;

namespace CalifornianHealth.WebBlazor.Models
{
    public class AvailabilityModel
    {
        public int Id { get; set; }
        public int ConsultantId { get; set; }
        public DateTime DateTime { get; set; }
    }
}