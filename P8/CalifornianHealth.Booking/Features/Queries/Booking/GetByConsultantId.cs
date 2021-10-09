using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Booking.Features.Queries.Booking
{
    public class GetByConsultantId
    {
        public record Query(int ConsultantId) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IBookingService _bookingService;

            public Handler(IBookingService bookingService)
            {
                _bookingService = bookingService;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                return new Response(await _bookingService.GetBookingByConsultantId(query.ConsultantId));
            }
        }

        public record Response(List<BookingDto> BookingDto);
    }
}