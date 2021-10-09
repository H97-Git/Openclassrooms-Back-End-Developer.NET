using Microsoft.AspNetCore.Identity;

namespace RestAPI.Models.Authentication.DTO
{
    public class UserDto : IdentityUser
    {
        public string Fullname { get; set; }
        public string Role { get; set; }
    }
}