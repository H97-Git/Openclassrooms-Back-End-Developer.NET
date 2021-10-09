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
    public class RatingControllerTests
    {
        private readonly RatingController _sut;
        private readonly IRatingService _ratingService = Substitute.For<IRatingService>();


        public RatingControllerTests()
        {
            _sut = new RatingController(_ratingService);
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
        public async Task GetRating_ShouldReturnList_WhenExist()
        {
            // Arrange
            _ratingService.GetRating().Returns(new List<RatingDto>());
            // Act
            var ratingDtos = await _sut.GetRating();

            // Assert
            ratingDtos.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRating_ShouldReturnRating_WhenExist()
        {
            // Arrange
            _ratingService.GetRating(99).Returns(new RatingDto { Id = 99,MoodyRating = "Unit Test" });

            // Act
            var ratingDto = await _sut.GetRating(99);

            // Assert
            ratingDto.Value.Id.Should().Be(99);
            ratingDto.Value.MoodyRating.Should().Be("Unit Test");
        }

        [Fact]
        public async Task GetRatingId_ShouldHandleException()
        {
            // Arrange
            const string message = "Rating not found in the database.";
            _ratingService.GetRating(Arg.Any<int>()).Throws(new KeyNotFoundException(message));

            // Act
            var actionResult = await _sut.GetRating(99);

            // Assert
            var objectResult = actionResult.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public void SaveRating_ShouldReturnRating()
        {
            // Arrange
            var ratingDto = new RatingDto {Id = 99};

            // Act
           var actionResult = _sut.PostRating(ratingDto);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            var value = result?.Value as RatingDto;
            value.Should().NotBeNull();
            value?.Id.Should().Be(99);
        }

        [Fact]
        public async void SaveRating_ShouldCallService()
        {
            // Arrange
            var counter = 0;
            var ratingDto = new RatingDto();
            _ratingService.When(x => x.SaveRating(Arg.Any<RatingDto>())).Do(x => counter++);

            // Act
            _sut.PostRating(ratingDto);

            // Assert
            await _ratingService.Received().SaveRating(ratingDto);
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateRating_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var ratingDto = new RatingDto() { Id = 99 };
            _ratingService.When(x => x.UpdateRating(ratingDto)).Do(x => counter++);
            _ratingService.GetRating(ratingDto.Id).Returns(ratingDto);

            // Act
            await _sut.PutRating(ratingDto.Id,ratingDto);

            // Assert
            await _ratingService.Received().UpdateRating(ratingDto);
            counter.Should().Be(1);
        }
    }
}