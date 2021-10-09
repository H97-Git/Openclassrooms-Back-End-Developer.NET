using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Booking.Features.Queries.Availability
{
    public static class GetAll
    {
        public record Query : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IAvailabilityService _availabilityService;

            public Handler(IAvailabilityService availabilityService)
            {
                _availabilityService = availabilityService;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                return new Response(await _availabilityService.GetAvailabilities());
            }
        }

        public record Response(List<AvailabilityDto> AvailabilityDto);
    }
}