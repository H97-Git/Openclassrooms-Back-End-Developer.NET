using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RestAPI.Controllers;
using RestAPI.Infrastructure.Services;
using RestAPI.Models.Authentication;
using RestAPI.Models.Authentication.DTO;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Controller
{
    public class UserControllerTests
    {
        private readonly UserController _sut;
        private readonly IUserService _userService = Substitute.For<IUserService>();


        public UserControllerTests()
        {
            _sut = new UserController(_userService);
            var user = new User
            {
                UserName = "UnitTest",
                Role = "User"
            };
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName),
                new("UserId", user.Id),
                new (ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };
            _sut.ControllerContext = context;
        }


        [Fact]
        public async Task GetUser_ShouldReturnList_WhenExist()
        {
            // Arrange
            _userService.GetUser().Returns(new List<UserDto>());
            // Act
            var userDtos = await _sut.GetUser();

            // Assert
            userDtos.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUser_ShouldReturnUser_WhenExist()
        {
            // Arrange
            _userService.GetUser(Arg.Any<string>()).Returns(new UserDto {UserName = "Unit Test"});

            // Act
            var userDto = await _sut.GetUser(Guid.NewGuid().ToString());

            // Assert
            userDto.Value.UserName.Should().Be("Unit Test");
        }

        [Fact]
        public async Task GetUserId_ShouldHandleException()
        {
            // Arrange
            const string message = "User not found in the database.";
            _userService.GetUser(Arg.Any<string>()).Throws(new KeyNotFoundException(message));
            // Act
            Func<Task> act = async () => await _sut.GetUser("99");
            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage(message);
        }

        [Fact]
        public void SaveUser_ShouldReturnUser()
        {
            // Arrange
            var userDto = new UserDto {UserName = "Unit Test"};

            // Act
           var actionResult = _sut.PostUser(userDto);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            var value = result?.Value as UserDto;
            value.Should().NotBeNull();
            value?.UserName.Should().Be("Unit Test");
        }

        [Fact]
        public async void SaveUser_ShouldCallService()
        {
            // Arrange
            var counter = 0;
            var userDto = new UserDto{UserName = "Unit Test"};
            _userService.When(x => x.SaveUser(Arg.Any<UserDto>())).Do(x => counter++);

            // Act
            var actionResult = _sut.PostUser(userDto);

            // Assert
            await _userService.Received().SaveUser(userDto);
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateUser_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var userDto = new UserDto{UserName = "Unit Test"};
            _userService.When(x => x.UpdateUser(userDto)).Do(x => counter++);
            _userService.GetUser(userDto.Id).Returns(userDto);

            // Act
            await _sut.PutUser(userDto.Id,userDto);

            // Assert
            await _userService.Received().UpdateUser(userDto);
            counter.Should().Be(1);
        }
    }
}