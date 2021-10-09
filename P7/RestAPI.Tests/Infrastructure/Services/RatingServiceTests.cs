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
    public class RatingServiceTests
    {
        private readonly RatingService _sut;
        private readonly IRatingRepository _ratingRepository = Substitute.For<IRatingRepository>();

        public RatingServiceTests()
        {
            _sut = new RatingService(_ratingRepository);
        }

        [Fact]
        public async Task GetRating_ShouldReturnEmptyList_WhenExist()
        {
            // Arrange
            _ratingRepository.GetRating().Returns(new List<Rating>());

            // Act
            var ratingsDto = await _sut.GetRating();

            // Assert
            ratingsDto.Should().BeEmpty();
        }

        [Fact]
        public async void GetRatingId_ShouldReturnRating_WhenExist()
        {
            // Arrange
            var rating = new Rating { Id = 99, MoodyRating = "Unit Test"};
            _ratingRepository.GetRating(rating.Id).Returns(rating);

            // Act
            var ratingDto = await _sut.GetRating(rating.Id);

            // Assert
            ratingDto.Should().NotBeNull();
            ratingDto.Id.Should().Be(99);
            ratingDto.MoodyRating.Should().Be("Unit Test");
        }

        [Fact]
        public void GetRatingId_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            _ratingRepository.GetRating(Arg.Any<int>()).ReturnsNull();

            // Act
            Func<Task> act = async () => await _sut.GetRating(new Random().Next());

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Rating not found in the database.");
        }

        [Fact]
        public async void SaveRating_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var ratingDto = new RatingDto();
            _ratingRepository.When(x => x.SaveRating(Arg.Any<Rating>())).Do(x => counter++);

            // Act
            await _sut.SaveRating(ratingDto);

            // Assert
            await _ratingRepository.Received().SaveRating(Arg.Any<Rating>());
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateRating_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var rating = new Rating { Id = 99 };
            _ratingRepository.When(x => x.UpdateRating(rating)).Do(x => counter++);
            _ratingRepository.GetRating(rating.Id).Returns(rating);

            var date = DateTime.Now;
            var ratingDto = new RatingDto { Id = 99, MoodyRating = "Unit Test"};

            // Act
            await _sut.UpdateRating(ratingDto);

            // Assert
            await _ratingRepository.Received().UpdateRating(rating);
            counter.Should().Be(1);
        }

        [Fact]
        public void UpdateRating_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            _ratingRepository.UpdateRating(Arg.Any<Rating>()).ReturnsNull();
            var ratingDto = new RatingDto() { Id = 99,MoodyRating = "Unit Test" };

            // Act
            Func<Task> act = async () => await _sut.UpdateRating(ratingDto);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Rating not found in the database.");
        }

        [Fact]
        public async void DeleteRating_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var rating = new Rating { Id = 99 };
            _ratingRepository.When(x => x.DeleteRating(rating.Id)).Do(x => counter++);
            _ratingRepository.GetRating(rating.Id).Returns(rating);

            // Act
            var ratingDto = new RatingDto { Id = 99 };
            await _sut.DeleteRating(ratingDto.Id);

            // Assert
            await _ratingRepository.Received().DeleteRating(rating.Id);
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteRating_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            _ratingRepository.DeleteRating(Arg.Any<int>()).ReturnsNull();

            // Act
            Func<Task> act = async () => await _sut.DeleteRating(new Random().Next());

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Rating not found in the database.");
        }
    }
}