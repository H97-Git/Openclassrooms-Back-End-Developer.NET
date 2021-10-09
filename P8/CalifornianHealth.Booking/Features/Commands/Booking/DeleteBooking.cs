using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Booking.Features.Commands.Booking
{
    public static class DeleteBooking
    {
        public record Command(int Id) : IRequest<int>;

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IBookingService _bookingService;

            public Handler(IBookingService bookingService)
            {
                _bookingService = bookingService;
            }

            public async Task<int> Handle(Command command, CancellationToken cancellationToken)
            {
                await _bookingService.DeleteBooking(command.Id);
                return command.Id;
            }
        }
    }
}