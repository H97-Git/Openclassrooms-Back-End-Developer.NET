using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Infrastructure.Services;
using RestAPI.Models;
using RestAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Infrastructure.Services
{
    public class CurvePointServiceTests
    {
        private readonly CurvePointService _sut;
        private readonly ICurvePointRepository _curvePointRepository = Substitute.For<ICurvePointRepository>();
        private readonly DateTime _date;

        public CurvePointServiceTests()
        {
            _sut = new CurvePointService(_curvePointRepository);
            _date = DateTime.Now;
        }

        [Fact]
        public async Task GetCurvePoint_ShouldReturnEmptyList_WhenExist()
        {
            // Arrange
            _curvePointRepository.GetCurvePoint().Returns(new List<CurvePoint>());

            // Act
            var curvePointsDto = await _sut.GetCurvePoint();

            // Assert
            curvePointsDto.Should().BeEmpty();
        }

        [Fact]
        public async void GetCurvePointId_ShouldReturnCurvePoint_WhenExist()
        {
            // Arrange
            var curvePoint = new CurvePoint { Id = 99,CreationDate = _date };
            _curvePointRepository.GetCurvePoint(curvePoint.Id).Returns(curvePoint);

            // Act
            var curvePointDto = await _sut.GetCurvePoint(curvePoint.Id);

            // Assert
            curvePointDto.Should().NotBeNull();
            curvePointDto.Id.Should().Be(99);
            curvePointDto.CreationDate.Should().Be(_date);
        }

        [Fact]
        public void GetCurvePointId_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            _curvePointRepository.GetCurvePoint(Arg.Any<int>()).ReturnsNull();

            // Act
            Func<Task> act = async () => await _sut.GetCurvePoint(new Random().Next());

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Curve point not found in the database.");
        }

        [Fact]
        public async void SaveCurvePoint_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var curvePointDto = new CurvePointDto();
            _curvePointRepository.When(x => x.SaveCurvePoint(Arg.Any<CurvePoint>())).Do(x => counter++);

            // Act
            await _sut.SaveCurvePoint(curvePointDto);

            // Assert
            await _curvePointRepository.Received().SaveCurvePoint(Arg.Any<CurvePoint>());
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateCurvePoint_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var curvePoint = new CurvePoint { Id = 99 };
            _curvePointRepository.When(x => x.UpdateCurvePoint(curvePoint)).Do(x => counter++);
            _curvePointRepository.GetCurvePoint(curvePoint.Id).Returns(curvePoint);

            var date = DateTime.Now;
            var curvePointDto = new CurvePointDto { Id = 99, CreationDate = date};

            // Act
            await _sut.UpdateCurvePoint(curvePointDto);

            // Assert
            await _curvePointRepository.Received().UpdateCurvePoint(curvePoint);
            counter.Should().Be(1);
        }

        [Fact]
        public void UpdateCurvePoint_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var date = DateTime.Now;
            var curvePointDto = new CurvePointDto { Id = 99, CreationDate = date};

            // Act
            Func<Task> act = async () => await _sut.UpdateCurvePoint(curvePointDto);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Curve point not found in the database.");
        }

        [Fact]
        public async void DeleteCurvePoint_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var curvePoint = new CurvePoint { Id = 99 };
            _curvePointRepository.When(x => x.DeleteCurvePoint(curvePoint.Id)).Do(x => counter++);
            _curvePointRepository.GetCurvePoint(curvePoint.Id).Returns(curvePoint);

            // Act
            var curvePointDto = new CurvePointDto { Id = 99 };
            await _sut.DeleteCurvePoint(curvePointDto.Id);

            // Assert
            await _curvePointRepository.Received().DeleteCurvePoint(curvePoint.Id);
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteCurvePoint_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var curvePointDto = new CurvePointDto { Id = 99 };

            // Act
            Func<Task> act = async () => await _sut.DeleteCurvePoint(curvePointDto.Id);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Curve point not found in the database.");
        }
    }
}