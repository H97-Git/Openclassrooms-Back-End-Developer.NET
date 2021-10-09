using System;
using System.Collections.Generic;
using System.Threading;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Features.Queries.Booking;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CalifornianHealth.Tests.Mediator.Queries
{
    public class BookingQueryHandlerTest
    {
        private readonly IBookingService _bookingService;
        private readonly CancellationToken _cancellationToken;
        private readonly int _r;

        public BookingQueryHandlerTest()
        {
            _bookingService = Substitute.For<IBookingService>();
            _r = new Random().Next();
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async void BookingGetHandler()
        {
            _bookingService.GetBooking(Arg.Any<int>()).Returns(new BookingDto {Id = 99});
            var sut = new Get.Handler(_bookingService);

            var query = new Get.Query(_r);
            var response = await sut.Handle(query, _cancellationToken);

            response.BookingDto.Id.Should().Be(99);
        }

        [Fact]
        public async void BookingGetByConsultantIdHandler()
        {
            _bookingService.GetBookingByConsultantId(Arg.Any<int>()).Returns(new List<BookingDto>());
            var sut = new GetByConsultantId.Handler(_bookingService);

            var query = new GetByConsultantId.Query(_r);
            var response = await sut.Handle(query, _cancellationToken);

            response.BookingDto.Should().BeEmpty();
        }

        [Fact]
        public async void BookingGetAllHandler()
        {
            _bookingService.GetBooking().Returns(new List<BookingDto>());
            var sut = new GetAll.Handler(_bookingService);

            var query = new GetAll.Query();
            var response = await sut.Handle(query, _cancellationToken);

            response.BookingDto.Should().BeEmpty();
        }
    }
}