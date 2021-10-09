using FluentAssertions;
using RestAPI.Infrastructure.Repositories;
using RestAPI.Models;
using System.Threading.Tasks;
using Xunit;

namespace RestAPI.Tests.Infrastructure.Repositories
{
    [Collection("SharedDbContext")]
    public class RuleRepositoryTests
    {
        private readonly DatabaseFixture _fixture;

        public RuleRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetRule_ShouldReturnList()
        {
            // Arrange
            var sut = new RuleRepository(_fixture.ApplicationDbContext);

            // Act
            var rules = await sut.GetRule();

            // Assert
            rules.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRule_ShouldReturnRule_WhenDoesExist()
        {
            // Arrange
            var sut = new RuleRepository(_fixture.InMemoryApplicationDbContext);

            // Act
            var rules = await sut.GetRule(5);

            // Assert
            rules.Should().NotBeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(999)]
        public async Task GetRule_ShouldReturnNull_WhenDoesNotExist(int id)
        {
            // Arrange
            var sut = new RuleRepository(_fixture.ApplicationDbContext);

            // Act
            var rule = await sut.GetRule(id);

            // Assert
            rule.Should().BeNull();
        }

        [Fact]
        public async Task CreateRule_ShouldAddEntity()
        {
            //Arrange
            var sut = new RuleRepository(_fixture.InMemoryApplicationDbContext);
            var ruleToPost = new Rule {Id = 5,Description = "Unit Test" };

            //Act
            await sut.SaveRule(ruleToPost);
            var rule = await sut.GetRule(5);

            //Assert
            rule.Should().NotBeNull();
            rule.Description.Should().Be("Unit Test");
        }

        [Fact]
        public async Task UpdateRule_ShouldChangeEntity()
        {
            //Arrange
            var sut = new RuleRepository(_fixture.InMemoryApplicationDbContext);
            var rule = await sut.GetRule(5);
            rule.Description = "Unit Test";

            //Act
            await sut.UpdateRule(rule);
            rule = await sut.GetRule(5);

            //Assert
            rule.Should().NotBeNull();
            rule.Description.Should().Be("Unit Test");
        }

        [Fact]
        public async Task DeleteRule_ShouldRemoveEntity()
        {
            //Arrange
            var sut = new RuleRepository(_fixture.InMemoryApplicationDbContext);
            await sut.SaveRule(new Rule {Id = 99, Description = "Unit Test"});
            var countBeforeDelete = (await sut.GetRule()).Count;
            //Act
            await sut.DeleteRule(1);
            var rules = await sut.GetRule();

            //Assert
            rules.Should().NotBeEmpty();
            rules.Count.Should().Be(countBeforeDelete - 1);
        }
    }
}