using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Repositories.Interface;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using Mapster;

namespace CalifornianHealth.Booking.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private static IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<BookingDto>> GetBooking()
        {
            var bookings = await _bookingRepository.GetBooking();

            return bookings.Adapt<List<BookingDto>>();
        }

        public async Task<BookingDto> GetBooking(int id)
        {
            var booking = await _bookingRepository.GetBooking(id);

            return booking is null
                ? throw new KeyNotFoundException($"Booking : {id}")
                : booking.Adapt<BookingDto>();
        }

        public async Task<List<BookingDto>> GetBookingByConsultantId(int consultantId)
        {
            var bookings = await _bookingRepository.GetBookingByConsultantId(consultantId);
            return bookings.Adapt<List<BookingDto>>();
        }

        public async Task UpdateBooking(BookingDto bookingDto)
        {
            if (bookingDto is null) throw new ArgumentNullException(nameof(bookingDto));

            var booking = await _bookingRepository.GetBooking(bookingDto.Id);
            if (booking is null) throw new KeyNotFoundException($"Booking : {bookingDto.Id}");

            bookingDto.Adapt(booking);
            await _bookingRepository.UpdateBooking(booking);
        }

        public async Task SaveBooking(BookingDto bookingDto)
        {
            if (bookingDto is null) throw new ArgumentNullException(nameof(bookingDto));

            var booking = bookingDto.Adapt<Data.Booking>();
            await _bookingRepository.SaveBooking(booking);
        }

        public async Task DeleteBooking(int id)
        {
            var booking = await _bookingRepository.GetBooking(id);
            if (booking is null) throw new KeyNotFoundException($"Booking : {id}");

            await _bookingRepository.DeleteBooking(booking);
        }
    }
}