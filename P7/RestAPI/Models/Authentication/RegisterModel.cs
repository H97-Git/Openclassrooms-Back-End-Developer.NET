using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.Authentication
{
    public class RegisterModel
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}