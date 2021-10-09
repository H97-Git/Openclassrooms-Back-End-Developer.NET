using System;
using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.BackgroundServices;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using EasyNetQ;
using MediatR;

namespace CalifornianHealth.Booking.Features.Commands.Booking
{
    public static class PostBooking
    {
        public record Command(BookingDto BookingDto) : IRequest<BookingDto>;

        public class Handler : IRequestHandler<Command, BookingDto>
        {
            private readonly IAvailabilityService _availabilityService;
            private readonly IBookingService _bookingService;
            private readonly IBus _bus;

            public Handler(IBookingService bookingService, IAvailabilityService availabilityService, IBus bus)
            {
                _bookingService = bookingService;
                _availabilityService = availabilityService;
                _bus = bus;
            }

            public async Task<BookingDto> Handle(Command command, CancellationToken cancellationToken)
            {
                await IsStillAvailable(command.BookingDto.AvailabilityId);

                await _availabilityService.DeleteAvailability(command.BookingDto.AvailabilityId);
                await _bookingService.SaveBooking(command.BookingDto);
                var list = await _bookingService.GetBooking();
                var bookingDto = list.Find(p => p.AvailabilityId == command.BookingDto.AvailabilityId);
                return bookingDto;
            }

            private async Task IsStillAvailable(int availabilityId)
            {
                var (_, exception) =
                    await _bus.Rpc.RequestAsync<BookingRequest, BookingResponse>(new BookingRequest(availabilityId));
                if (exception is not null) throw new Exception("Unavailable");
            }
        }
    }
}