using System;
using System.Collections.Generic;
using System.Threading;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Features.Commands.Availability;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CalifornianHealth.Tests.Mediator.Commands
{
    public class AvailabilityHandler
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly CancellationToken _cancellationToken;
        private readonly int _r;

        public AvailabilityHandler()
        {
            _availabilityService = Substitute.For<IAvailabilityService>();
            _r = new Random().Next();
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async void AvailabilityPostHandler()
        {
            var availabilityDto = new AvailabilityDto {Id = 99};
            _availabilityService.GetAvailabilities().Returns(new List<AvailabilityDto> {availabilityDto});
            var sut = new PostAvailability.Handler(_availabilityService);

            var command = new PostAvailability.Command(availabilityDto);
            var assert = await sut.Handle(command, _cancellationToken);

            assert.Id.Should().Be(99);
            await _availabilityService.Received().GetAvailabilities();
            await _availabilityService.Received().SaveAvailability(availabilityDto);
        }

        [Fact]
        public async void AvailabilityPutHandler()
        {
            var availabilityDto = new AvailabilityDto {Id = 99};
            _availabilityService.GetAvailability(Arg.Any<int>()).Returns(availabilityDto);
            var sut = new PutAvailability.Handler(_availabilityService);

            var command = new PutAvailability.Command(availabilityDto);
            var assert = await sut.Handle(command, _cancellationToken);

            assert.Id.Should().Be(99);
            await _availabilityService.Received().GetAvailability(Arg.Any<int>());
            await _availabilityService.Received().UpdateAvailability(availabilityDto);
        }

        [Fact]
        public async void AvailabilityDeleteHandler()
        {
            var availabilityDto = new AvailabilityDto {Id = 99};
            var sut = new DeleteAvailability.Handler(_availabilityService);

            var command = new DeleteAvailability.Command(99);
            var assert = await sut.Handle(command, _cancellationToken);

            assert.Should().Be(99);
            await _availabilityService.Received().DeleteAvailability(availabilityDto.Id);
        }
    }
}