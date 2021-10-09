using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Repositories.Interface;
using CalifornianHealth.Booking.Infrastructure.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CalifornianHealth.Tests.Infrastructure.Services
{
    public class BookingServiceTest
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly BookingService _sut;

        public BookingServiceTest()
        {
            _bookingRepository = Substitute.For<IBookingRepository>();
            _sut = new BookingService(_bookingRepository);
        }

        [Fact]
        public async Task GetBookings()
        {
            _bookingRepository.GetBooking().Returns(new List<Booking.Data.Booking>());

            var bookings = await _sut.GetBooking();

            bookings.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBooking()
        {
            var booking = new Booking.Data.Booking {Id = 99, ConsultantId = 99};
            _bookingRepository.GetBooking(booking.Id).Returns(booking);

            var bookingDto = await _sut.GetBooking(booking.Id);

            bookingDto.Should().NotBeNull();
        }

        [Fact]
        public async Task GetBookingByConsultantId()
        {
            _bookingRepository.GetBookingByConsultantId(Arg.Any<int>()).Returns(new List<Booking.Data.Booking>());

            var bookingDto = await _sut.GetBookingByConsultantId(new Random().Next());

            bookingDto.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateBooking()
        {
            var booking = new Booking.Data.Booking {Id = 99, ConsultantId = 99};
            var counter = 0;
            _bookingRepository.GetBooking(booking.Id).Returns(booking);
            _bookingRepository
                .When(x => x.UpdateBooking(booking))
                .Do(x => counter++);

            var bookingDto = new BookingDto {Id = 99, ConsultantId = 100};
            await _sut.UpdateBooking(bookingDto);

            await _bookingRepository.Received().UpdateBooking(booking);
            counter.Should().Be(1);
        }

        [Fact]
        public void UpdateBooking_ShouldThrowArgumentNullException_WhenNull()
        {
            Func<Task> act = async () => await _sut.UpdateBooking(null);

            act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public void UpdateBooking_ShouldThrowKeyNotFoundException_WhenNotFound()
        {
            _bookingRepository.GetBooking(Arg.Any<int>()).ReturnsNull();
            var bookingDto = new BookingDto();

            Func<Task> act = async () => await _sut.UpdateBooking(bookingDto);
            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Booking : {bookingDto.Id}");
        }

        [Fact]
        public async Task SaveBooking()
        {
            var bookingDto = new BookingDto {Id = 99, ConsultantId = 99};
            var counter = 0;
            _bookingRepository
                .When(p => p.SaveBooking(Arg.Any<Booking.Data.Booking>()))
                .Do(x => counter++);

            await _sut.SaveBooking(bookingDto);

            await _bookingRepository.Received().SaveBooking(Arg.Any<Booking.Data.Booking>());
            counter.Should().Be(1);
        }

        [Fact]
        public void SaveBooking_ShouldThrowArgumentNullException_WhenNull()
        {
            Func<Task> act = async () => await _sut.SaveBooking(null);

            act.Should().ThrowAsync<ArgumentNullException>();
        }


        [Fact]
        public async Task DeleteBooking()
        {
            var counter = 0;
            _bookingRepository
                .When(p => p.DeleteBooking(Arg.Any<Booking.Data.Booking>()))
                .Do(x => counter++);
            _bookingRepository.GetBooking(Arg.Any<int>()).Returns(new Booking.Data.Booking());

            await _sut.DeleteBooking(1);

            await _bookingRepository.Received().DeleteBooking(Arg.Any<Booking.Data.Booking>());
            counter.Should().Be(1);
        }

        [Fact]
        public void DeleteBooking_ShouldThrowKeyNotFoundException_WhenNotFound()
        {
            _bookingRepository.GetBooking(Arg.Any<int>()).ReturnsNull();
            var bookingDto = new BookingDto();

            Func<Task> act = async () => await _sut.DeleteBooking(1);
            act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Booking : {bookingDto.Id}");
        }
    }
}