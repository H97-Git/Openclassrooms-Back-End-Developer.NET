using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models.Authentication;

namespace RestAPI.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<IList<User>> GetUser();

        Task<User> GetUser(string id);

        Task UpdateUser(User user);

        Task SaveUser(User user);

        Task DeleteUser(string id);
    }
}