using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Booking.Features.Commands.Availability
{
    public static class PostAvailability
    {
        public record Command(AvailabilityDto AvailabilityDto) : IRequest<AvailabilityDto>;

        public class Handler : IRequestHandler<Command, AvailabilityDto>
        {
            private readonly IAvailabilityService _availabilityService;

            public Handler(IAvailabilityService availabilityService)
            {
                _availabilityService = availabilityService;
            }

            public async Task<AvailabilityDto> Handle(Command command, CancellationToken cancellationToken)
            {
                await _availabilityService.SaveAvailability(command.AvailabilityDto);
                var list = await _availabilityService.GetAvailabilities();
                var availabilityDto = list.Find(p => p.Id == command.AvailabilityDto.Id);
                return availabilityDto;
            }
        }
    }
}