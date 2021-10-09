using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Booking.Features.Commands.Booking
{
    public static class PutBooking
    {
        public record Command(BookingDto BookingDto) : IRequest<BookingDto>;

        public class Handler : IRequestHandler<Command, BookingDto>
        {
            private readonly IBookingService _bookingService;

            public Handler(IBookingService bookingService)
            {
                _bookingService = bookingService;
            }

            public async Task<BookingDto> Handle(Command command, CancellationToken cancellationToken)
            {
                await _bookingService.UpdateBooking(command.BookingDto);
                return await _bookingService.GetBooking(command.BookingDto.Id);
            }
        }
    }
}