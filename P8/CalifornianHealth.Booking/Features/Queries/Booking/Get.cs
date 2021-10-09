using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Booking.Features.Queries.Booking
{
    public static class Get
    {
        public record Query(int Id) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IBookingService _bookingService;

            public Handler(IBookingService bookingService)
            {
                _bookingService = bookingService;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                var booking = await _bookingService.GetBooking(query.Id);
                return new Response(booking);
            }
        }

        public record Response(BookingDto BookingDto);
    }
}