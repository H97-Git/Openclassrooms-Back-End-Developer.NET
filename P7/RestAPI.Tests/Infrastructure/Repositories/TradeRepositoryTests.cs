using FluentAssertions;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Infrastructure.Repositories
{
    [Collection("SharedDbContext")]
    public class TradeRepositoryTests
    {
        private readonly DatabaseFixture _fixture;

        public TradeRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetTrade_ShouldReturnList()
        {
            // Arrange
            var sut = new TradeRepository(_fixture.ApplicationDbContext);

            // Act
            var trades = await sut.GetTrade();

            // Assert
            trades.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTrade_ShouldReturnTrade_WhenDoesExist()
        {
            // Arrange
            var sut = new TradeRepository(_fixture.InMemoryApplicationDbContext);

            // Act
            var trade = await sut.GetTrade(5);

            // Assert
            trade.Should().NotBeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        public async Task GetTrade_ShouldReturnNull_WhenTradeDoesNotExist(int id)
        {
            // Arrange
            var sut = new TradeRepository(_fixture.ApplicationDbContext);

            // Act
            var trade = await sut.GetTrade(id);

            // Assert
            trade.Should().BeNull();
        }

        [Fact]
        public async Task CreateTrade_ShouldAddEntity()
        {
            //Arrange
            var sut = new TradeRepository(_fixture.InMemoryApplicationDbContext);
            var tradeToPost = new Trade {Id = 5,Account = "Unit Test" };

            //Act
            await sut.SaveTrade(tradeToPost);
            var trade = await sut.GetTrade(5);

            //Assert
            trade.Should().NotBeNull();
            trade.Account.Should().Be("Unit Test");
        }

        [Fact]
        public async Task UpdateTrade_ShouldChangeEntity()
        {
            //Arrange
            var sut = new TradeRepository(_fixture.InMemoryApplicationDbContext);
            var trade = await sut.GetTrade(1);
            trade.Account = "Unit Test";

            //Act
            await sut.UpdateTrade(trade);
            trade = await sut.GetTrade(1);

            //Assert
            trade.Should().NotBeNull();
            trade.Account.Should().Be("Unit Test");
        }

        [Fact]
        public async Task DeleteTrade_ShouldRemoveEntity()
        {
            //Arrange
            var sut = new TradeRepository(_fixture.InMemoryApplicationDbContext);
            await sut.SaveTrade(new Trade {Id = 99, Account = "Unit Test"});
            var countBeforeDelete = (await sut.GetTrade()).Count;

            //Act
            await sut.DeleteTrade(1);
            var trades = await sut.GetTrade();

            //Assert
            trades.Should().NotBeEmpty();
            trades.Count.Should().Be(countBeforeDelete - 1);
        }
    }
}