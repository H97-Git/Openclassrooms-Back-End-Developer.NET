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
    public class BidControllerTests
    {
        private readonly BidController _sut;
        private readonly IBidService _bidService = Substitute.For<IBidService>();


        public BidControllerTests()
        {
            _sut = new BidController(_bidService);
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
        public async Task GetBid_ShouldReturnList_WhenExist()
        {
            // Arrange
            _bidService.GetBid().Returns(new List<BidDto>());
            // Act
            var bidDtos = await _sut.GetBid();

            // Assert
            bidDtos.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBid_ShouldReturnBid_WhenExist()
        {
            // Arrange
            _bidService.GetBid(99).Returns(new BidDto { Id = 99,Account = "Unit Test" });

            // Act
            var bidDto = await _sut.GetBid(99);

            // Assert
            bidDto.Value.Id.Should().Be(99);
            bidDto.Value.Account.Should().Be("Unit Test");
        }

        [Fact]
        public async Task GetBidId_ShouldHandleException()
        {
            // Arrange
            const string message = "Bid not found in the database.";
            _bidService.GetBid(Arg.Any<int>()).Throws(new KeyNotFoundException(message));

            // Act
            var actionResult = await _sut.GetBid(99);

            // Assert
            var objectResult = actionResult.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public void SaveBid_ShouldReturnBid()
        {
            // Arrange
            var bidDto = new BidDto {Id = 99};

            // Act
            var actionResult = _sut.PostBid(bidDto);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            var value = result?.Value as BidDto;
            value.Should().NotBeNull();
            value?.Id.Should().Be(99);
        }

        [Fact]
        public async void SaveBid_ShouldCallService()
        {
            // Arrange
            var counter = 0;
            var bidDto = new BidDto();
            var user = (_sut.HttpContext.User.FindFirst("UserId")?.Value, _sut.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
            _bidService.When(x => x.SaveBid(user,Arg.Any<BidDto>())).Do(x => counter++);

            // Act
            _sut.PostBid(bidDto);

            // Assert
            await _bidService.Received().SaveBid(user,bidDto);
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateBid_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var bidDto = new BidDto() { Id = 99 };
            var user =  (_sut.HttpContext.User.FindFirst("UserId")?.Value, _sut.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
            _bidService.When(x => x.UpdateBid(user,bidDto)).Do(x => counter++);
            _bidService.GetBid(bidDto.Id).Returns(bidDto);

            // Act
            await _sut.PutBid(bidDto.Id,bidDto);

            // Assert
            await _bidService.Received().UpdateBid(user,bidDto);
            counter.Should().Be(1);
        }
    }
}