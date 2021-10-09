using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RestAPI.Controllers;
using RestAPI.Infrastructure.Services;
using RestAPI.Models.Authentication;
using RestAPI.Models.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Controller
{
    public class RuleControllerTests
    {
        private readonly RuleController _sut;
        private readonly IRuleService _ruleService = Substitute.For<IRuleService>();


        public RuleControllerTests()
        {
            _sut = new RuleController(_ruleService);
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
        public async Task GetRule_ShouldReturnList_WhenExist()
        {
            // Arrange
            _ruleService.GetRule().Returns(new List<RuleDto>());
            // Act
            var ruleDtos = await _sut.GetRule();

            // Assert
            ruleDtos.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRule_ShouldReturnRule_WhenExist()
        {
            // Arrange
            _ruleService.GetRule(99).Returns(new RuleDto { Id = 99,Description = "Unit Test" });

            // Act
            var ruleDto = await _sut.GetRule(99);

            // Assert
            ruleDto.Value.Id.Should().Be(99);
            ruleDto.Value.Description.Should().Be("Unit Test");
        }

        [Fact]
        public async Task GetRuleId_ShouldHandleException()
        {
            // Arrange
            const string message = "Rule not found in the database.";
            _ruleService.GetRule(Arg.Any<int>()).Throws(new KeyNotFoundException(message));

            // Act
            var actionResult = await _sut.GetRule(99);

            // Assert
            var objectResult = actionResult.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public void SaveRule_ShouldReturnRule()
        {
            // Arrange
            var ruleDto = new RuleDto {Id = 99};

            // Act
           var actionResult = _sut.PostRule(ruleDto);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            var value = result?.Value as RuleDto;
            value.Should().NotBeNull();
            value?.Id.Should().Be(99);
        }

        [Fact]
        public async void SaveRule_ShouldCallService()
        {
            // Arrange
            var counter = 0;
            var ruleDto = new RuleDto();
            _ruleService.When(x => x.SaveRule(Arg.Any<RuleDto>())).Do(x => counter++);

            // Act
            _sut.PostRule(ruleDto);

            // Assert
            await _ruleService.Received().SaveRule(ruleDto);
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateRule_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var ruleDto = new RuleDto() { Id = 99 };
            _ruleService.When(x => x.UpdateRule(ruleDto)).Do(x => counter++);
            _ruleService.GetRule(ruleDto.Id).Returns(ruleDto);

            // Act
            await _sut.PutRule(ruleDto.Id,ruleDto);

            // Assert
            await _ruleService.Received().UpdateRule(ruleDto);
            counter.Should().Be(1);
        }
    }
}