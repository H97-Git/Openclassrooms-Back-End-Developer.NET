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
    public class TradeControllerTests
    {
        private readonly TradeController _sut;
        private readonly ITradeService _tradeService = Substitute.For<ITradeService>();


        public TradeControllerTests()
        {
            _sut = new TradeController(_tradeService);
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
        public async Task GetTrade_ShouldReturnList_WhenExist()
        {
            // Arrange
            _tradeService.GetTrade().Returns(new List<TradeDto>());
            // Act
            var tradeDtos = await _sut.GetTrade();

            // Assert
            tradeDtos.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTrade_ShouldReturnTrade_WhenExist()
        {
            // Arrange
            _tradeService.GetTrade(99).Returns(new TradeDto { Id = 99,Account = "Unit Test" });

            // Act
            var tradeDto = await _sut.GetTrade(99);

            // Assert
            tradeDto.Value.Id.Should().Be(99);
            tradeDto.Value.Account.Should().Be("Unit Test");
        }

        [Fact]
        public async Task GetTradeId_ShouldHandleException()
        {
            // Arrange
            const string message = "Trade not found in the database.";
            _tradeService.GetTrade(Arg.Any<int>()).Throws(new KeyNotFoundException(message));

            // Act
            var actionResult = await _sut.GetTrade(99);

            // Assert
            var objectResult = actionResult.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public void SaveTrade_ShouldReturnTrade()
        {
            // Arrange
            var tradeDto = new TradeDto {Id = 99};

            // Act
           var actionResult = _sut.PostTrade(tradeDto);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            var value = result?.Value as TradeDto;
            value.Should().NotBeNull();
            value?.Id.Should().Be(99);
        }

        [Fact]
        public async void SaveTrade_ShouldCallService()
        {
            // Arrange
            var counter = 0;
            var TradeDto = new TradeDto();
            var user = (_sut.HttpContext.User.FindFirst("UserId")?.Value, _sut.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
            _tradeService.When(x => x.SaveTrade(user,Arg.Any<TradeDto>())).Do(x => counter++);

            // Act
            _sut.PostTrade(TradeDto);

            // Assert
            await _tradeService.Received().SaveTrade(user,TradeDto);
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateTrade_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var TradeDto = new TradeDto() { Id = 99 };
            var user =  (_sut.HttpContext.User.FindFirst("UserId")?.Value, _sut.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
            _tradeService.When(x => x.UpdateTrade(user,TradeDto)).Do(x => counter++);
            _tradeService.GetTrade(TradeDto.Id).Returns(TradeDto);

            // Act
            await _sut.PutTrade(TradeDto.Id,TradeDto);

            // Assert
            await _tradeService.Received().UpdateTrade(user,TradeDto);
            counter.Should().Be(1);
        }
    }
}