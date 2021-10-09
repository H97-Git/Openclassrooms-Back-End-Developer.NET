using FluentAssertions;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Infrastructure.Repositories
{
    [Collection("SharedDbContext")]
    public class CurvePointRepositoryTests
    {
        private readonly DatabaseFixture _fixture;

        public CurvePointRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetCurvePoint_ShouldReturnList()
        {
            // Arrange
            var sut = new CurvePointRepository(_fixture.ApplicationDbContext);

            // Act
            var curvePoints = await sut.GetCurvePoint();

            // Assert
            curvePoints.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCurvePoint_ShouldReturnCurvePoint_WhenDoesExist()
        {
            // Arrange
            var sut = new CurvePointRepository(_fixture.InMemoryApplicationDbContext);

            // Act
            var curvePoint = await sut.GetCurvePoint(1);

            // Assert
            curvePoint.Should().NotBeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        public async Task GetCurvePoint_ShouldReturnNull_WhenCurvePointDoesNotExist(int id)
        {
            // Arrange
            var sut = new CurvePointRepository(_fixture.ApplicationDbContext);

            // Act
            var curvePoint = await sut.GetCurvePoint(id);

            // Assert
            curvePoint.Should().BeNull();
        }

        [Fact]
        public async Task CreateCurvePoint_ShouldAddEntity()
        {
            //Arrange
            var sut = new CurvePointRepository(_fixture.InMemoryApplicationDbContext);
            var date = DateTime.Now;
            var curvePointToPost = new CurvePoint { Id = 5,CreationDate = date };

            //Act
            await sut.SaveCurvePoint(curvePointToPost);
            var curvePoint = await sut.GetCurvePoint(5);

            //Assert
            curvePoint.Should().NotBeNull();
            curvePoint.CreationDate.Should().Be(date);
        }

        [Fact]
        public async Task UpdateCurvePoint_ShouldChangeEntity()
        {
            //Arrange
            var sut = new CurvePointRepository(_fixture.InMemoryApplicationDbContext);
            var date = DateTime.Now;
            var curvePoint = await sut.GetCurvePoint(1);
            curvePoint.CreationDate = date;
            //Act
            await sut.UpdateCurvePoint(curvePoint);
            curvePoint = await sut.GetCurvePoint(1);

            //Assert
            curvePoint.Should().NotBeNull();
            curvePoint.CreationDate.Should().Be(date);
        }

        [Fact]
        public async Task DeleteCurvePoint_ShouldRemoveEntity()
        {
            //Arrange
            var sut = new CurvePointRepository(_fixture.InMemoryApplicationDbContext);
            await sut.SaveCurvePoint(new CurvePoint(){Id = 99,CreationDate = DateTime.Now});
            var countBeforeDelete = (await sut.GetCurvePoint()).Count;

            //Act
            await sut.DeleteCurvePoint(99);
            var curvePoints = await sut.GetCurvePoint();

            //Assert
            curvePoints.Should().NotBeEmpty();
            curvePoints.Count.Should().Be(countBeforeDelete - 1);
        }
    }
}