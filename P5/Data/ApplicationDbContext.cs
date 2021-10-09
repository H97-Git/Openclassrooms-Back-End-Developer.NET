using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace The_Car_Hub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<InventoryStatus> InventoryStatus { get; set; }
    }
}