using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Infrastructure.Services;
using RestAPI.Models;
using RestAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Infrastructure.Services
{
    public class BidServiceTests
    {
        private readonly BidService _sut;

        private readonly IBidRepository _bidRepository = Substitute.For<IBidRepository>();

        public BidServiceTests()
        {
            _sut = new BidService(_bidRepository);
        }

        [Fact]
        public async Task GetBid_ShouldReturnEmptyList_WhenExist()
        {
            // Arrange
            _bidRepository.GetBid().Returns(new List<Bid>());

            // Act
            var bidsDto = await _sut.GetBid();

            // Assert
            bidsDto.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBidId_ShouldReturnBid_WhenExist()
        {
            // Arrange
            var bid = new Bid { Id = 99,Account = "Unit Test" };
            _bidRepository.GetBid(bid.Id).Returns(bid);

            // Act
            var bidDto = await _sut.GetBid(bid.Id);

            // Assert
            bidDto.Should().NotBeNull();
            bidDto.Id.Should().Be(99);
            bidDto.Account.Should().Be("Unit Test");
        }

        [Fact]
        public void GetBidId_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            _bidRepository.GetBid(Arg.Any<int>()).ReturnsNull();

            // Act
            Func<Task> act = async () => await _sut.GetBid(new Random().Next());

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Bid not found in the database.");
        }

        [Fact]
        public async Task SaveBid_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var user = (Guid.NewGuid().ToString(), "Admin");
            var bidDto = new BidDto();
            _bidRepository.When(x => x.SaveBid(Arg.Any<Bid>())).Do(x => counter++);

            // Act
            await _sut.SaveBid(user,bidDto);

            // Assert
            await _bidRepository.Received().SaveBid(Arg.Any<Bid>());
            counter.Should().Be(1);
        }

        [Fact]
        public async Task UpdateBid_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var user = (Guid.NewGuid().ToString(), "Admin");
            var bid = new Bid { Id = 99 };
            _bidRepository.When(x => x.UpdateBid(bid)).Do(x => counter++);
            _bidRepository.GetBid(bid.Id).Returns(bid);

            // Act
            var bidDto = new BidDto { Id = 99,Account = "Unit Test" };
            await _sut.UpdateBid(user,bidDto);

            // Assert
            await _bidRepository.Received().UpdateBid(bid);
            counter.Should().Be(1);
        }

        [Fact]
        public void UpdateBid_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var user = (Guid.NewGuid().ToString(), "Admin");
            var bidDto = new BidDto { Id = 99,Account = "Unit Test" };

            // Act
            Func<Task> act = async () => await _sut.UpdateBid(user,bidDto);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Bid not found in the database.");
        }

        [Fact]
        public void UpdateBid_ShouldThrowAuthenticationException_WhenUserIsNotOwner()
        {
            // Arrange
            var bid = new Bid { Id = 99, OwnerId = "Unit Test" };
            _bidRepository.GetBid(bid.Id).Returns(bid);
            var user = (Guid.NewGuid().ToString(), "User");
            var bidDto = new BidDto { Id = 99 };

            // Act
            Func<Task> act = async () => await _sut.UpdateBid(user, bidDto);

            // Assert
            act.Should().Throw<AuthenticationException>()
                .WithMessage("Owner only can edit this bid.");
        }

        [Fact]
        public async Task DeleteBid_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var user = (Guid.NewGuid().ToString(), "Admin");
            var bid = new Bid { Id = 99 };
            _bidRepository.When(x => x.DeleteBid(bid.Id)).Do(x => counter++);
            _bidRepository.GetBid(bid.Id).Returns(bid);

            // Act
            var bidDto = new BidDto { Id = 99,Account = "Unit Test" };
            await _sut.DeleteBid(user,bidDto.Id);

            // Assert
            await _bidRepository.Received().DeleteBid(bid.Id);
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteBid_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var user = (Guid.NewGuid().ToString(), "Admin");
            var bidDto = new BidDto { Id = 99,Account = "Unit Test" };

            // Act
            Func<Task> act = async () => await _sut.DeleteBid(user,bidDto.Id);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Bid not found in the database.");
        }

        [Fact]
        public void DeleteBid_ShouldThrowAuthenticationException_WhenUserIsNotOwner()
        {
            // Arrange
            var bid = new Bid { Id = 99,OwnerId = "Unit Test" };
            _bidRepository.GetBid(bid.Id).Returns(bid);
            var user = (Guid.NewGuid().ToString(), "User");
            var bidDto = new BidDto { Id = 99 };

            // Act
            Func<Task> act = async () => await _sut.DeleteBid(user,bidDto.Id);

            // Assert
            act.Should().Throw<AuthenticationException>()
                .WithMessage("Owner only can delete this bid.");
        }
    }
}