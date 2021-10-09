using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Repositories.Interface;
using CalifornianHealth.Booking.Infrastructure.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CalifornianHealth.Tests.Infrastructure.Services
{
    public class AvailabilityServiceTest
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly AvailabilityService _sut;

        public AvailabilityServiceTest()
        {
            _availabilityRepository = Substitute.For<IAvailabilityRepository>();
            _sut = new AvailabilityService(_availabilityRepository);
        }

        [Fact]
        public async Task GetAvailabilities()
        {
            _availabilityRepository.GetAvailabilities().Returns(new List<Availability>());

            var bookings = await _sut.GetAvailabilities();

            bookings.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAvailability()
        {
            var availability = new Availability {Id = 99, ConsultantId = 99};
            _availabilityRepository.GetAvailability(availability.Id).Returns(availability);

            var availabilityDto = await _sut.GetAvailability(availability.Id);

            availabilityDto.Should().NotBeNull();
        }

        [Fact]
        public async Task FilterByConsultantId()
        {
            _availabilityRepository.FilterByConsultantId(Arg.Any<int>()).Returns(new List<Availability>());

            var availabilityDto = await _sut.FilterByConsultantId(new Random().Next());

            availabilityDto.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAvailability()
        {
            var availability = new Availability {Id = 99, ConsultantId = 99};
            var counter = 0;
            _availabilityRepository.GetAvailability(availability.Id).Returns(availability);
            _availabilityRepository
                .When(x => x.UpdateAvailability(availability))
                .Do(x => counter++);

            var availabilityDto = new AvailabilityDto {Id = 99, ConsultantId = 100};
            await _sut.UpdateAvailability(availabilityDto);

            await _availabilityRepository.Received().UpdateAvailability(availability);
            counter.Should().Be(1);
        }

        [Fact]
        public void UpdateBooking_ShouldThrowArgumentNullException_WhenNull()
        {
            Func<Task> act = async () => await _sut.UpdateAvailability(null);

            act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public void UpdateBooking_ShouldThrowKeyNotFoundException_WhenNotFound()
        {
            _availabilityRepository.GetAvailability(Arg.Any<int>()).ReturnsNull();
            var availabilityDto = new AvailabilityDto();

            Func<Task> act = async () => await _sut.UpdateAvailability(availabilityDto);
            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Booking : {availabilityDto.Id}");
        }

        [Fact]
        public async Task SaveAvailability()
        {
            var availabilityDto = new AvailabilityDto {Id = 99, ConsultantId = 99};
            var counter = 0;
            _availabilityRepository
                .When(x => x.SaveAvailability(Arg.Any<Availability>()))
                .Do(x => counter++);

            await _sut.SaveAvailability(availabilityDto);

            await _availabilityRepository.Received().SaveAvailability(Arg.Any<Availability>());
            counter.Should().Be(1);
        }

        [Fact]
        public void SaveBooking_ShouldThrowArgumentNullException_WhenNull()
        {
            Func<Task> act = async () => await _sut.SaveAvailability(null);

            act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteAvailability()
        {
            var counter = 0;
            _availabilityRepository
                .When(x => x.DeleteAvailability(Arg.Any<Availability>()))
                .Do(x => counter++);
            _availabilityRepository
                .GetAvailability(Arg.Any<int>())
                .Returns(new Availability());

            await _sut.DeleteAvailability(1);

            await _availabilityRepository.Received().DeleteAvailability(Arg.Any<Availability>());
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteBooking_ShouldThrowKeyNotFoundException_WhenNotFound()
        {
            _availabilityRepository.GetAvailability(Arg.Any<int>()).ReturnsNull();
            var bookingDto = new BookingDto();

            Func<Task> act = async () => await _sut.DeleteAvailability(1);
            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Booking : {bookingDto.Id}");
        }
    }
}