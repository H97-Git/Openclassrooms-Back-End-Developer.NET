using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Authentication;

namespace RestAPI.Models
{
    public class IdentityDbContext : IdentityDbContext<User>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }
    }
}