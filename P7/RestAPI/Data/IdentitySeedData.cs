using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using RestAPI.Models.Authentication;

namespace RestAPI.Data
{
    public static class IdentitySeedData
    {
        private const string AdminUser = "Admin";
        private const string Password = "P@ssword123";

        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var userManager =
                scope.ServiceProvider.GetService(typeof(UserManager<User>)) as UserManager<User>;

            var roleManager =
                scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>)) as RoleManager<IdentityRole>;

            if (userManager != null)
            {
                await EnsureRole(roleManager);

                var userList = userManager.Users.ToList();
                if (userList.Count == 0)
                {
                    userList = EnsureUserList();
                    foreach (var user in userList)
                    {
                        await userManager.CreateAsync(user,Password);
                        switch (user.Role)
                        {
                            case "Admin":
                                await userManager.AddToRoleAsync(user,"Admin");
                                break;

                            case "User":
                                await userManager.AddToRoleAsync(user,"User");
                                break;
                        }
                    }
                }
            }
        }

        public static async Task EnsureRole(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(AdminUser))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminUser));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }

        public static List<User> EnsureUserList()
        {
            return new List<User>
            {
                new User
                {
                    UserName = AdminUser,
                    EmailConfirmed = true,
                    Email = "admin@api.com",
                    Role = AdminUser,
                },
                new User
                {
                    UserName = "userPost",
                    EmailConfirmed = true,
                    Email = "bobPost@api.com",
                    Role = "User"
                },
                new User
                {
                    UserName = "userPut",
                    EmailConfirmed = true,
                    Email = "bobPut@api.com",
                    Role = "User"
                }
            };
        }
    }
}