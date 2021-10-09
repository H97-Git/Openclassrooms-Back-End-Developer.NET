using Microsoft.EntityFrameworkCore;

namespace CalifornianHealth.Demographics.Data
{
    public class DemographicsContext : DbContext
    {
        public DemographicsContext(DbContextOptions<DemographicsContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
    }
}