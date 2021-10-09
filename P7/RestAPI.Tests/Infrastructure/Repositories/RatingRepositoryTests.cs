using FluentAssertions;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Infrastructure.Repositories
{
    [Collection("SharedDbContext")]
    public class RatingRepositoryTests
    {
        private readonly DatabaseFixture _fixture;

        public RatingRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetRating_ShouldReturnList()
        {
            // Arrange
            var sut = new RatingRepository(_fixture.ApplicationDbContext);

            // Act
            var ratings = await sut.GetRating();

            // Assert
            ratings.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRating_ShouldReturnRating_WhenDoesExist()
        {
            // Arrange
            var sut = new RatingRepository(_fixture.InMemoryApplicationDbContext);

            // Act
            var rating = await sut.GetRating(5);

            // Assert
            rating.Should().NotBeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        public async Task GetRating_ShouldReturnNull_WhenRatingDoesNotExist(int id)
        {
            // Arrange
            var sut = new RatingRepository(_fixture.ApplicationDbContext);

            // Act
            var rating = await sut.GetRating(id);

            // Assert
            rating.Should().BeNull();
        }

        [Fact]
        public async Task CreateRating_ShouldAddEntity()
        {
            //Arrange
            var sut = new RatingRepository(_fixture.InMemoryApplicationDbContext);
            var ratingToPost = new Rating { Id = 5,MoodyRating = "Unit Test" };

            //Act
            await sut.SaveRating(ratingToPost);
            var rating = await sut.GetRating(5);

            //Assert
            rating.Should().NotBeNull();
            rating.MoodyRating.Should().Be("Unit Test");
        }

        [Fact]
        public async Task UpdateRating_ShouldChangeEntity()
        {
            //Arrange
            var sut = new RatingRepository(_fixture.InMemoryApplicationDbContext);
            var rating = await sut.GetRating(5);
            rating.FitchRating = "Unit Test";

            //Act
            await sut.UpdateRating(rating);
            rating = await sut.GetRating(5);

            //Assert
            rating.Should().NotBeNull();
            rating.FitchRating.Should().Be("Unit Test");
        }

        [Fact]
        public async Task DeleteRating_ShouldRemoveEntity()
        {
            //Arrange
            var sut = new RatingRepository(_fixture.InMemoryApplicationDbContext);
            await sut.SaveRating(new Rating {Id = 99, MoodyRating = "Unit Test"});
            var countBeforeDelete = (await sut.GetRating()).Count;
            //Act
            await sut.DeleteRating(1);
            var ratings = await sut.GetRating();

            //Assert
            ratings.Should().NotBeEmpty();
            ratings.Count.Should().Be(countBeforeDelete - 1);
        }
    }
}