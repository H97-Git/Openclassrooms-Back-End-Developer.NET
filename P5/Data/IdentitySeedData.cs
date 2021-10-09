using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace The_Car_Hub.Models
{
    public class IdentitySeedData
    {
        private const string AdminUser = "Admin@thecarhub.com";
        private const string AdminPassword = "Pwd123!";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = (UserManager<IdentityUser>)scope.ServiceProvider.GetService(typeof(UserManager<IdentityUser>));

                IdentityUser user = await userManager.FindByIdAsync(AdminUser);

                if (user == null)
                {
                    user = new IdentityUser(AdminUser);
                    user.EmailConfirmed = true;
                    await userManager.CreateAsync(user,AdminPassword);
                }
            }
        }
    }
}