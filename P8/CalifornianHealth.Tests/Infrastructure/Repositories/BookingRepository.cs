using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Infrastructure.Repositories;
using FluentAssertions;
using Xunit;

namespace CalifornianHealth.Tests.Infrastructure.Repositories
{
    [Collection("SharedDbContext")]
    public class BookingRepositoryTest
    {
        private readonly DatabaseFixture _fixture;

        public BookingRepositoryTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetBookings()
        {
            var sut = new BookingRepository(_fixture.Context);

            var bookings = await sut.GetBooking();

            bookings.Should()
                .NotBeNullOrEmpty()
                .And.HaveCount(599)
                .And.ContainItemsAssignableTo<Booking.Data.Booking>()
                .And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task GetBooking()
        {
            var sut = new BookingRepository(_fixture.Context);

            var booking = await sut.GetBooking(1);

            booking.Should().BeOfType<Booking.Data.Booking>();
        }

        [Fact]
        public async Task GetBookingByConsultantId()
        {
            var sut = new BookingRepository(_fixture.Context);

            var bookings = await sut.GetBookingByConsultantId(1);

            bookings.Should()
                .NotBeNullOrEmpty()
                .And.ContainItemsAssignableTo<Booking.Data.Booking>()
                .And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task SaveBooking()
        {
            var sut = new BookingRepository(_fixture.Context);
            var booking = new Booking.Data.Booking
            {
                AvailabilityId = 0,
                ConsultantId = 0,
                PatientId = 0,
                Appointment = DateTime.Now
            };

            await sut.SaveBooking(booking);
            IEnumerable<Booking.Data.Booking> bookings = await sut.GetBooking();

            bookings.Should()
                .NotBeNullOrEmpty()
                .And.HaveCount(600)
                .And.ContainItemsAssignableTo<Booking.Data.Booking>()
                .And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task UpdateBooking()
        {
            var sut = new BookingRepository(_fixture.Context);
            var booking = await sut.GetBooking(1);

            booking.Appointment = DateTime.Now;
            await sut.UpdateBooking(booking);

            var updatedBooking = await sut.GetBooking(1);
            booking.Should().Be(updatedBooking);
        }

        [Fact]
        public async Task DeleteBooking()
        {
            var sut = new BookingRepository(_fixture.Context);
            var booking = await sut.GetBooking(1);

            await sut.DeleteBooking(booking);
            var bookings = await sut.GetBooking();

            bookings.Should()
                .NotBeNullOrEmpty()
                .And.HaveCount(598)
                .And.ContainItemsAssignableTo<Booking.Data.Booking>()
                .And.OnlyHaveUniqueItems();
        }
    }
}