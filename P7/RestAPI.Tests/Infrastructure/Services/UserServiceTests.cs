using System;
using FluentAssertions;
using Mapster;
using NSubstitute;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Infrastructure.Services;
using RestAPI.Models.Authentication;
using RestAPI.Models.Authentication.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Infrastructure.Services
{
    public class UserServiceTests
    {
        private readonly IUserRepository _userRepository;
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _sut = new UserService(_userRepository);
        }

        [Fact]
        public async Task GetUser_ShouldReturnList()
        {

           var userList = new List<User>()
             {
                 new(){UserName = "Admin",Role = "Admin"},
                 new(){UserName = "Unit Test2",Role = "User"},
                 new(){UserName = "Unit Test3",Role = "User"},
                 new(){UserName = "Unit Test4",Role = "User"},
                 new(){UserName = "Unit Test5",Role = "User"}
             };

            // Arrange
            _userRepository.GetUser().Returns(userList);

            // Act
            var users = await _sut.GetUser();

            // Assert
            users.Should().NotBeNullOrEmpty()
                .And.BeEquivalentTo(userList)
                .And.HaveCount(5)
                .And.OnlyHaveUniqueItems()
                .And.ContainItemsAssignableTo<UserDto>();

        }

        [Fact]
        public async Task GetUser()
        {
            // Arrange
            var user = new User { UserName = "Admin" };
            _userRepository.GetUser(user.Id).Returns(user);

            // Act
            var userDto = await _sut.GetUser(user.Id);

            // Assert
            userDto.UserName.Should().Be("Admin");
            await _userRepository.Received(1).GetUser(user.Id);
        }

        [Fact]
        public void GetUser_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var userDto = new UserDto { UserName = "Admin" };
            // Act
            Func<Task> act = async () => await _sut.GetUser(userDto.Id);
            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("User not found in the database.");
        }

        [Fact]
        public async void CreateUser_ShouldAddEntity()
        {
            // Arrange
            var userDto = new UserDto() { UserName = "Admin" };

            // Act
            await _sut.SaveUser(userDto);

            // Assert
            await _userRepository.Received().SaveUser(Arg.Any<User>());
        }

        [Fact]
        public async void UpdateUser_ShouldChangeEntity()
        {
            // Arrange
            var counter = 0;
            var user = new User { UserName = "Admin" };
            _userRepository.GetUser(Arg.Any<string>()).Returns(user);
            _userRepository.When(x => x.UpdateUser(Arg.Any<User>())).Do(x => counter++);
            // Act
            await _sut.UpdateUser(user.Adapt<UserDto>());
            // Assert
            await _userRepository.Received().UpdateUser(Arg.Any<User>());
            counter.Should().Be(1);
        }

        [Fact]
        public void UpdateUser_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var userDto = new UserDto { UserName = "Admin" };
            // Act
            Func<Task> act = async () => await _sut.UpdateUser(userDto);
            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("User not found in the database.");
        }

        [Fact]
        public async void DeleteUser_ShouldRemoveEntity()
        {
            // Arrange
            var user = new User { UserName = "Admin" };
            _userRepository.GetUser(user.Id).Returns(user);
            // Act
            await _sut.DeleteUser(user.Id);
            // Assert
            await _userRepository.Received().DeleteUser(user.Id);
        }

        [Fact]
        public void DeleteUser_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var userDto = new UserDto { UserName = "Admin" };
            // Act
            Func<Task> act = async () => await _sut.DeleteUser(userDto.Id);
            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("User not found in the database.");
        }
    }
}
