using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models.Authentication;
using RestAPI.Models.Authentication.DTO;
using RestAPI.Properties;

namespace RestAPI.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IList<UserDto>> GetUser()
        {
            var users = await _userRepository.GetUser();

            return users.Adapt<IList<UserDto>>();
        }

        public async Task<UserDto> GetUser(string id)
        {
            var user = await _userRepository.GetUser(id);
            if (user == null)
            {
                throw new KeyNotFoundException(Resources.UserNotFound);
            }

            return user.Adapt<UserDto>();
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var user = await _userRepository.GetUser(userDto.Id);
            if (user == null)
            {
                throw new KeyNotFoundException(Resources.UserNotFound);
            }

            userDto.Adapt(user);

            await _userRepository.UpdateUser(user);
        }

        public async Task SaveUser(UserDto userDto)
        {
            var user = userDto.Adapt<User>();
            await _userRepository.SaveUser(user);
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userRepository.GetUser(id);
            if (user == null)
            {
                throw new KeyNotFoundException(Resources.UserNotFound);
            }

            await _userRepository.DeleteUser(id);
        }
    }
}