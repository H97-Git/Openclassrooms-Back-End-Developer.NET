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
    public class TradeServiceTests
    {
        private readonly TradeService _sut;

        private readonly ITradeRepository _tradeRepository = Substitute.For<ITradeRepository>();

        public TradeServiceTests()
        {
            _sut = new TradeService(_tradeRepository);
        }

        [Fact]
        public async Task GetTrade_ShouldReturnEmptyList_WhenExist()
        {
            // Arrange
            _tradeRepository.GetTrade().Returns(new List<Trade>());

            // Act
            var tradesDto = await _sut.GetTrade();

            // Assert
            tradesDto.Should().BeEmpty();
        }

        [Fact]
        public async void GetTradeId_ShouldReturnTrade_WhenTradeExist()
        {
            // Arrange
            var trade = new Trade { Id = 99,Account = "Unit Test" };
            _tradeRepository.GetTrade(trade.Id).Returns(trade);

            // Act
            var tradeDto = await _sut.GetTrade(trade.Id);

            // Assert
            tradeDto.Should().NotBeNull();
            tradeDto.Id.Should().Be(99);
            tradeDto.Account.Should().Be("Unit Test");
        }

        [Fact]
        public void GetTradeId_ShouldThrowKeyNotFoundException_WhenTradeDoesNotExist()
        {
            // Arrange
            _tradeRepository.GetTrade(Arg.Any<int>()).ReturnsNull();

            // Act
            Func<Task> act = async () => await _sut.GetTrade(new Random().Next());

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Trade not found in the database.");
        }

        [Fact]
        public async void SaveTrade_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var user = (Guid.NewGuid().ToString(), "Admin");
            var tradeDto = new TradeDto();
            _tradeRepository.When(x => x.SaveTrade(Arg.Any<Trade>())).Do(x => counter++);

            // Act
            await _sut.SaveTrade(user,tradeDto);

            // Assert
            await _tradeRepository.Received().SaveTrade(Arg.Any<Trade>());
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateTrade_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var user = (Guid.NewGuid().ToString(), "Admin");
            var trade = new Trade { Id = 99 };
            _tradeRepository.When(x => x.UpdateTrade(trade)).Do(x => counter++);
            _tradeRepository.GetTrade(trade.Id).Returns(trade);

            // Act
            var tradeDto = new TradeDto { Id = 99,Account = "Unit Test" };
            await _sut.UpdateTrade(user,tradeDto);

            // Assert
            await _tradeRepository.Received().UpdateTrade(trade);
            counter.Should().Be(1);
        }

        [Fact]
        public void UpdateTrade_ShouldThrowKeyNotFoundException_WhenTradeDoesNotExist()
        {
            // Arrange
            var user = (Guid.NewGuid().ToString(), "Admin");
            var tradeDto = new TradeDto { Id = 99,Account = "Unit Test" };

            // Act
            Func<Task> act = async () => await _sut.UpdateTrade(user,tradeDto);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Trade not found in the database.");
        }

        [Fact]
        public void UpdateTrade_ShouldThrowAuthenticationException_WhenUserIsNotOwner()
        {
            // Arrange
            var trade = new Trade { Id = 99,OwnerId = "Unit Test" };
            _tradeRepository.GetTrade(trade.Id).Returns(trade);
            var user = (Guid.NewGuid().ToString(), "User");
            var tradeDto = new TradeDto { Id = 99 };

            // Act
            Func<Task> act = async () => await _sut.UpdateTrade(user,tradeDto);

            // Assert
            act.Should().Throw<AuthenticationException>()
                .WithMessage("Owner only can edit this Trade.");
        }

        [Fact]
        public async void DeleteTrade_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var user = (Guid.NewGuid().ToString(), "Admin");
            var trade = new Trade { Id = 99 };
            _tradeRepository.When(x => x.DeleteTrade(trade.Id)).Do(x => counter++);
            _tradeRepository.GetTrade(trade.Id).Returns(trade);

            // Act
            var tradeDto = new TradeDto { Id = 99,Account = "Unit Test" };
            await _sut.DeleteTrade(user,tradeDto.Id);

            // Assert
            await _tradeRepository.Received().DeleteTrade(trade.Id);
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteTrade_ShouldThrowKeyNotFoundException_WhenTradeDoesNotExist()
        {
            // Arrange
            var user = (Guid.NewGuid().ToString(), "Admin");
            var tradeDto = new TradeDto { Id = 99,Account = "Unit Test" };

            // Act
            Func<Task> act = async () => await _sut.DeleteTrade(user,tradeDto.Id);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Trade not found in the database.");
        }

        [Fact]
        public void DeleteTrade_ShouldThrowAuthenticationException_WhenUserIsNotOwner()
        {
            // Arrange
            var trade = new Trade { Id = 99,OwnerId = "Unit Test" };
            _tradeRepository.GetTrade(trade.Id).Returns(trade);
            var user = (Guid.NewGuid().ToString(), "User");
            var tradeDto = new TradeDto { Id = 99 };

            // Act
            Func<Task> act = async () => await _sut.DeleteTrade(user,tradeDto.Id);

            // Assert
            act.Should().Throw<AuthenticationException>()
                .WithMessage("Owner only can delete this Trade.");
        }
    }
}