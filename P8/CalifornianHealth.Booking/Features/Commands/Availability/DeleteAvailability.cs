using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Booking.Features.Commands.Availability
{
    public static class DeleteAvailability
    {
        public record Command(int Id) : IRequest<int>;

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IAvailabilityService _availabilityService;

            public Handler(IAvailabilityService availabilityService)
            {
                _availabilityService = availabilityService;
            }

            public async Task<int> Handle(Command command, CancellationToken cancellationToken)
            {
                await _availabilityService.DeleteAvailability(command.Id);
                return command.Id;
            }
        }
    }
}