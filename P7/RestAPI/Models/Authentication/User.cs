using Microsoft.AspNetCore.Identity;

namespace RestAPI.Models.Authentication
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public string Role { get; set; }
    }
}