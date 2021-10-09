using System;
using System.Collections.Generic;
using System.Threading;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Features.Queries.Availability;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CalifornianHealth.Tests.Mediator.Queries
{
    public class AvailabilityQueryHandler
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly CancellationToken _cancellationToken;
        private readonly int _r;

        public AvailabilityQueryHandler()
        {
            _availabilityService = Substitute.For<IAvailabilityService>();
            _r = new Random().Next();
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async void AvailabilityGetHandler()
        {
            _availabilityService.GetAvailability(Arg.Any<int>()).Returns(new AvailabilityDto {Id = 99});
            var sut = new Get.Handler(_availabilityService);

            var query = new Get.Query(_r);
            var response = await sut.Handle(query, _cancellationToken);

            response.AvailabilityDto.Id.Should().Be(99);
        }

        [Fact]
        public async void AvailabilityGetByConsultantIdHandler()
        {
            _availabilityService.FilterByConsultantId(Arg.Any<int>()).Returns(new List<AvailabilityDto>());
            var sut = new GetByConsultantId.Handler(_availabilityService);

            var query = new GetByConsultantId.Query(_r);
            var response = await sut.Handle(query, _cancellationToken);

            response.AvailabilityDto.Should().BeEmpty();
        }

        [Fact]
        public async void AvailabilityGetAllHandler()
        {
            _availabilityService.GetAvailabilities().Returns(new List<AvailabilityDto>());
            var sut = new GetAll.Handler(_availabilityService);

            var query = new GetAll.Query();
            var response = await sut.Handle(query, _cancellationToken);

            response.AvailabilityDto.Should().BeEmpty();
        }
    }
}