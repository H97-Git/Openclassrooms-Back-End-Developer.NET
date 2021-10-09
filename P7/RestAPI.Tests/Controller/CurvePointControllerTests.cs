using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RestAPI.Controllers;
using RestAPI.Infrastructure.Services;
using RestAPI.Models.Authentication;
using RestAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Controller
{
    public class CurvePointControllerTests
    {
        private readonly CurvePointController _sut;
        private readonly ICurvePointService _curvePointService = Substitute.For<ICurvePointService>();


        public CurvePointControllerTests()
        {
            _sut = new CurvePointController(_curvePointService);
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
        public async Task GetCurvePoint_ShouldReturnList_WhenExist()
        {
            // Arrange
            _curvePointService.GetCurvePoint().Returns(new List<CurvePointDto>());
            // Act
            var curvePointDtos = await _sut.GetCurvePoint();

            // Assert
            curvePointDtos.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCurvePoint_ShouldReturnCurvePoint_WhenExist()
        {
            // Arrange
            var date = DateTime.Now;
            _curvePointService.GetCurvePoint(99).Returns(new CurvePointDto { Id = 99,CreationDate = date });

            // Act
            var curvePointDto = await _sut.GetCurvePoint(99);

            // Assert
            curvePointDto.Value.Id.Should().Be(99);
            curvePointDto.Value.CreationDate.Should().Be(date);
        }

        [Fact]
        public async Task GetCurvePointId_ShouldHandleException()
        {
            // Arrange
            const string message = "CurvePoint not found in the database.";
            _curvePointService.GetCurvePoint(Arg.Any<int>()).Throws(new KeyNotFoundException(message));

            // Act
            var actionResult = await _sut.GetCurvePoint(99);

            // Assert
            var objectResult = actionResult.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public void SaveCurvePoint_ShouldReturnCurvePoint()
        {
            // Arrange
            var curvePointDto = new CurvePointDto {Id = 99};

            // Act
           var actionResult = _sut.PostCurvePoint(curvePointDto);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            var value = result?.Value as CurvePointDto;
            value.Should().NotBeNull();
            value?.Id.Should().Be(99);
        }

        [Fact]
        public async void SaveCurvePoint_ShouldCallService()
        {
            // Arrange
            var counter = 0;
            var curvePointDto = new CurvePointDto();
            _curvePointService.When(x => x.SaveCurvePoint(Arg.Any<CurvePointDto>())).Do(x => counter++);

            // Act
            _sut.PostCurvePoint(curvePointDto);

            // Assert
            await _curvePointService.Received().SaveCurvePoint(curvePointDto);
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateCurvePoint_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var curvePointDto = new CurvePointDto() { Id = 99 };
            _curvePointService.When(x => x.UpdateCurvePoint(curvePointDto)).Do(x => counter++);
            _curvePointService.GetCurvePoint(curvePointDto.Id).Returns(curvePointDto);

            // Act
            await _sut.PutCurvePoint(curvePointDto.Id,curvePointDto);

            // Assert
            await _curvePointService.Received().UpdateCurvePoint(curvePointDto);
            counter.Should().Be(1);
        }
    }
}