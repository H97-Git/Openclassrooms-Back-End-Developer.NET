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
    public class RuleServiceTests
    {
        private readonly RuleService _sut;
        private readonly IRuleRepository _ruleRepository = Substitute.For<IRuleRepository>();

        public RuleServiceTests()
        {
            _sut = new RuleService(_ruleRepository);
        }

        [Fact]
        public async Task GetRule_ShouldReturnEmptyList_WhenExist()
        {
            // Arrange
            _ruleRepository.GetRule().Returns(new List<Rule>());

            // Act
            var rulesDto = await _sut.GetRule();

            // Assert
            rulesDto.Should().BeEmpty();
        }

        [Fact]
        public async void GetRuleId_ShouldReturnRule_WhenRuleExist()
        {
            // Arrange
            var rule = new Rule { Id = 99, Description = "Unit Test"};
            _ruleRepository.GetRule(rule.Id).Returns(rule);

            // Act
            var ruleDto = await _sut.GetRule(rule.Id);

            // Assert
            ruleDto.Should().NotBeNull();
            ruleDto.Id.Should().Be(99);
            ruleDto.Description.Should().Be("Unit Test");
        }

        [Fact]
        public void GetRuleId_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var ruleDto = new RuleDto { Id = 99, Description = "Unit Test"};
            _ruleRepository.GetRule(Arg.Any<int>()).ReturnsNull();

            // Act
            Func<Task> act = async () => await _sut.UpdateRule(ruleDto);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Rule not found in the database.");
        }

        [Fact]
        public async void SaveRule_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var ruleDto = new RuleDto();
            _ruleRepository.When(x => x.SaveRule(Arg.Any<Rule>())).Do(x => counter++);

            // Act
            await _sut.SaveRule(ruleDto);

            // Assert
            await _ruleRepository.Received().SaveRule(Arg.Any<Rule>());
            counter.Should().Be(1);
        }

        [Fact]
        public async void UpdateRule_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var rule = new Rule { Id = 99 };
            _ruleRepository.When(x => x.UpdateRule(rule)).Do(x => counter++);
            _ruleRepository.GetRule(rule.Id).Returns(rule);

            var ruleDto = new RuleDto { Id = 99, Description = "Unit Test"};

            // Act
            await _sut.UpdateRule(ruleDto);

            // Assert
            await _ruleRepository.Received().UpdateRule(rule);
            counter.Should().Be(1);
        }
        
        [Fact]
        public void UpdateRule_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            var ruleDto = new RuleDto { Id = 99, Description = "Unit Test"};
            _ruleRepository.UpdateRule(Arg.Any<Rule>()).ReturnsNull();

            // Act
            Func<Task> act = async () => await _sut.UpdateRule(ruleDto);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Rule not found in the database.");
        }

        [Fact]
        public async void DeleteRule_ShouldCallRepo()
        {
            // Arrange
            var counter = 0;
            var rule = new Rule { Id = 99 };
            _ruleRepository.When(x => x.DeleteRule(rule.Id)).Do(x => counter++);
            _ruleRepository.GetRule(rule.Id).Returns(rule);

            // Act
            var ruleDto = new RuleDto { Id = 99 };
            await _sut.DeleteRule(ruleDto.Id);

            // Assert
            await _ruleRepository.Received().DeleteRule(rule.Id);
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteRule_ShouldThrowKeyNotFoundException_WhenDoesNotExist()
        {
            // Arrange
            _ruleRepository.DeleteRule(Arg.Any<int>()).ReturnsNull();

            // Act
            Func<Task> act = async () => await _sut.DeleteRule(new Random().Next());

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Rule not found in the database.");
        }

    }
}