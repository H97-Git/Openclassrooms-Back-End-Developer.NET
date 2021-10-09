using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Authentication;

namespace RestAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<User>> GetUser()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task UpdateUser([Required] User user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task SaveUser([Required] User user)
        {
            await _userManager.CreateAsync(user);
        }

        public async Task DeleteUser(string id)
        {
            var user = await GetUser(id);
            await _userManager.DeleteAsync(user);
        }
    }
}