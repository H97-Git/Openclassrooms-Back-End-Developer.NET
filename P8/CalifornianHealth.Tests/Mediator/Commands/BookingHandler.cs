using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.BackgroundServices;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Features.Commands.Booking;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using EasyNetQ;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CalifornianHealth.Tests.Mediator.Commands
{
    public class BookingCommandHandlerTest
    {
        private readonly IBookingService _bookingService;
        private readonly CancellationToken _cancellationToken;
        private readonly int _r;
        private readonly BookingDto _bookingDto;

        public BookingCommandHandlerTest()
        {
            _bookingService = Substitute.For<IBookingService>();
            _r = new Random().Next();
            _cancellationToken = new CancellationToken();
            _bookingDto = new BookingDto {Id = 99,AvailabilityId = 99};
        }

        [Fact]
        public async void BookingPostHandler()
        {
            var availabilityService = Substitute.For<IAvailabilityService>();
            var bus = Substitute.For<IBus>();

            _bookingService.GetBooking().Returns(new List<BookingDto> {_bookingDto});
            bus.Rpc.RequestAsync<BookingRequest, BookingResponse>(new BookingRequest(_bookingDto.AvailabilityId), cancellationToken: _cancellationToken)
                .Returns(new BookingResponse(true));
            
            var sut = new PostBooking.Handler(_bookingService,availabilityService,bus);
            
            var command = new PostBooking.Command(_bookingDto);
            var assert = await sut.Handle(command, _cancellationToken);

            assert.Should().Be(_bookingDto);
        }
        
        [Fact]
        public async void BookingPostErrorHandler()
        {
            var availabilityService = Substitute.For<IAvailabilityService>();
            var bus = Substitute.For<IBus>();

            _bookingService.GetBooking().Returns(new List<BookingDto> {_bookingDto});
            bus.Rpc.RequestAsync<BookingRequest, BookingResponse>(new BookingRequest(_bookingDto.AvailabilityId), cancellationToken: _cancellationToken)
                .Returns(new BookingResponse(true,new Exception()));
            
            var sut = new PostBooking.Handler(_bookingService,availabilityService,bus);
            
            var command = new PostBooking.Command(_bookingDto);

            Func<Task> act = async () => await sut.Handle(command, _cancellationToken);
            await act.Should().ThrowAsync<Exception>().WithMessage("Unavailable");
        }

        [Fact]
        public async void BookingPutHandler()
        {
            _bookingService.GetBooking(Arg.Any<int>()).Returns(_bookingDto);
            var sut = new PutBooking.Handler(_bookingService);

            var command = new PutBooking.Command(_bookingDto);
            var assert = await sut.Handle(command, _cancellationToken);

            assert.Id.Should().Be(99);
            await _bookingService.Received().GetBooking(Arg.Any<int>());
            await _bookingService.Received().UpdateBooking(_bookingDto);
        }

        [Fact]
        public async void BookingDeleteHandler()
        {
            var sut = new DeleteBooking.Handler(_bookingService);

            var command = new DeleteBooking.Command(99);
            var assert = await sut.Handle(command, _cancellationToken);

            assert.Should().Be(99);
            await _bookingService.Received().DeleteBooking(_bookingDto.Id);
        }
    }
}