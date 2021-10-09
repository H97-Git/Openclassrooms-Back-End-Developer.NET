using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data;
using CalifornianHealth.Booking.Infrastructure.Repositories;
using FluentAssertions;
using Xunit;

namespace CalifornianHealth.Tests.Infrastructure.Repositories
{
    [Collection("SharedDbContext")]
    public class AvailabilityRepositoryTest
    {
        private readonly DatabaseFixture _fixture;

        public AvailabilityRepositoryTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAvailabilities()
        {
            var sut = new AvailabilityRepository(_fixture.Context);

            IEnumerable<Availability> availabilities = await sut.GetAvailabilities();

            availabilities.Should()
                .NotBeNullOrEmpty()
                .And.HaveCount(599)
                .And.ContainItemsAssignableTo<Availability>()
                .And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task GetAvailability()
        {
            var sut = new AvailabilityRepository(_fixture.Context);

            var availability = await sut.GetAvailability(1);

            availability.Should().NotBeNull();
        }

        [Fact]
        public async Task FilterByConsultantId()
        {
            var sut = new AvailabilityRepository(_fixture.Context);

            var availabilities = await sut.FilterByConsultantId(1);

            availabilities.Should()
                .NotBeNullOrEmpty()
                .And.ContainItemsAssignableTo<Availability>()
                .And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task SaveAvailability()
        {
            var sut = new AvailabilityRepository(_fixture.Context);
            var availability = new Availability
            {
                ConsultantId = 99,
                DateTime = DateTime.Now
            };

            await sut.SaveAvailability(availability);
            IEnumerable<Availability> availabilities = await sut.GetAvailabilities();

            availabilities.Should()
                .NotBeNullOrEmpty()
                .And.HaveCount(600)
                .And.ContainItemsAssignableTo<Availability>()
                .And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task UpdateAvailability()
        {
            var sut = new AvailabilityRepository(_fixture.Context);
            var availability = await sut.GetAvailability(1);

            availability.ConsultantId = 99;
            availability.DateTime = DateTime.Now;
            await sut.UpdateAvailability(availability);

            var updatedAvailability = await sut.GetAvailability(1);
            availability.Should().Be(updatedAvailability);
        }

        [Fact]
        public async Task DeleteAvailability()
        {
            var sut = new AvailabilityRepository(_fixture.Context);
            var availability = await sut.GetAvailability(1);

            await sut.DeleteAvailability(availability);
            var availabilities = await sut.GetAvailabilities();

            availabilities.Should()
                .NotBeNullOrEmpty()
                .And.HaveCount(598)
                .And.ContainItemsAssignableTo<Availability>()
                .And.OnlyHaveUniqueItems();
        }
    }
}