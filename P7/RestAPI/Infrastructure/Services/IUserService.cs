using System.Collections.Generic;
using System.Threading.Tasks;
using RestAPI.Models.Authentication.DTO;

namespace RestAPI.Infrastructure.Services
{
    public interface IUserService
    {
        Task<IList<UserDto>> GetUser();

        Task<UserDto> GetUser(string id);

        Task UpdateUser(UserDto userDto);

        Task SaveUser(UserDto userDto);

        Task DeleteUser(string id);
    }
}