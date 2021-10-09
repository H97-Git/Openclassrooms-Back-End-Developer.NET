using FluentAssertions;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Infrastructure.Repositories
{
    [Collection("SharedDbContext")]
    public class BidRepositoryTests
    {
        private readonly DatabaseFixture _fixture;

        public BidRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetBid_ShouldReturnList()
        {
            // Arrange
            var sut = new BidRepository(_fixture.ApplicationDbContext);

            // Act
            var bids = await sut.GetBid();

            // Assert
            bids.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBid_ShouldReturnBid_WhenDoesExist()
        {
            // Arrange
            var sut = new BidRepository(_fixture.InMemoryApplicationDbContext);

            // Act
            var bid = await sut.GetBid(1);

            // Assert
            bid.Should().NotBeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        public async Task GetBid_ShouldReturnNull_WhenDoesNotExist(int id)
        {
            // Arrange
            var sut = new BidRepository(_fixture.ApplicationDbContext);

            // Act
            var bid = await sut.GetBid(id);

            // Assert
            bid.Should().BeNull();
        }

        [Fact]
        public async Task CreateBid_ShouldAddEntity()
        {
            //Arrange
            var sut = new BidRepository(_fixture.InMemoryApplicationDbContext);

            //Act
            await sut.SaveBid(new Bid { Account = "Unit Test" });
            var bid = await sut.GetBid(5);

            //Assert
            bid.Should().NotBeNull();
            bid.Account.Should().Be("Unit Test");
        }

        [Fact]
        public async Task UpdateBid_ShouldChangeEntity()
        {
            //Arrange
            var sut = new BidRepository(_fixture.InMemoryApplicationDbContext);
            var bid = await sut.GetBid(1);
            bid.Account = "Unit Test";

            //Act
            await sut.UpdateBid(bid);
            bid = await sut.GetBid(1);
            //Assert
            bid.Should().NotBeNull();
            bid.Account.Should().Be("Unit Test");
        }

        [Fact]
        public async Task DeleteBid_ShouldRemoveEntity()
        {
            //Arrange
            var sut = new BidRepository(_fixture.InMemoryApplicationDbContext);
            await sut.SaveBid(new Bid{Id = 99,Account = "99"});
            var countBeforeDelete = (await sut.GetBid()).Count;

            //Act
            await sut.DeleteBid(99);
            var bids = await sut.GetBid();

            //Assert
            bids.Should().NotBeEmpty();
            bids.Count.Should().Be(countBeforeDelete - 1);
        }
    }
}