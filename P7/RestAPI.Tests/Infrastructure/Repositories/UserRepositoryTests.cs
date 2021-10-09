using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MockQueryable.NSubstitute;
using NSubstitute;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Infrastructure.Repositories
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _sut;
        private readonly UserManager<User> _userManager;

        public UserRepositoryTests()
        {
            var store = Substitute.For<IUserStore<User>>();
            _userManager = Substitute.For<UserManager<User>>(store, null, null, null, null, null, null, null, null);

            _sut = new UserRepository(_userManager);
        }

        [Fact]
        public async Task GetUser_ShouldReturnList()
        {
            // Arrange
            var userList = new List<User>()
            {
                new(){UserName = "Admin",Role = "Admin"},
                new(){UserName = "Unit Test2",Role = "User"},
                new(){UserName = "Unit Test3",Role = "User"},
                new(){UserName = "Unit Test4",Role = "User"},
                new(){UserName = "Unit Test5",Role = "User"}
            };
            var queryable = userList.AsQueryable().BuildMock();

            _userManager.Users.Returns(queryable);

            // Act
            var users = await _sut.GetUser();

            // Assert
            users.Should().BeEquivalentTo(userList);
        }

        [Fact]
        public async Task GetUser_ShouldReturnUser_WhenExist()
        {
            // Arrange
            _userManager.FindByIdAsync(Arg.Any<string>()).Returns(new User { UserName = "Admin" });

            // Act
            var user = await _sut.GetUser("Admin");

            // Assert
            user.UserName.Should().Be("Admin");
        }

        [Fact]
        public async void CreateUser_ShouldAddEntity()
        {
            // Arrange
            var counter = 0;
            var user = new User { UserName = "Admin" };
            _userManager.When(x => x.CreateAsync(Arg.Any<User>())).Do(x => counter++);

            // Act
            await _sut.SaveUser(user);

            // Assert
            await _userManager.Received().CreateAsync(user);
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateUser_ShouldChangeEntity()
        {
            // Arrange
            var counter = 0;
            var user = new User { UserName = "Admin" };
            _userManager.When(x => x.UpdateAsync(Arg.Any<User>())).Do(x => counter++);
            // Act
            await _sut.UpdateUser(user);
            // Assert
            await _userManager.Received().UpdateAsync(user);
            counter.Should().Be(1);
        }

        [Fact]
        public async void DeleteUser_ShouldRemoveEntity()
        {
            // Arrange
            var counter = 0;
            var user = new User {UserName = "Admin"};
            _userManager.FindByIdAsync(Arg.Any<string>()).Returns(user);
            _userManager.When(x => x.DeleteAsync(Arg.Any<User>())).Do(x => counter++);
            // Act
            await _sut.DeleteUser("");
            // Assert
            await _userManager.Received().DeleteAsync(user);
            counter.Should().Be(1);
        }
    }
}