using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data.DTO;
using CalifornianHealth.Demographics.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Demographics.Features.Commands.Consultant
{
    public static class PostConsultant
    {
        public record Command(ConsultantDto ConsultantDto) : IRequest<ConsultantDto>;

        public class Handler : IRequestHandler<Command, ConsultantDto>
        {
            private readonly IConsultantService _consultantService;

            public Handler(IConsultantService consultantService)
            {
                _consultantService = consultantService;
            }

            public async Task<ConsultantDto> Handle(Command command, CancellationToken cancellationToken)
            {
                await _consultantService.SaveConsultant(command.ConsultantDto);
                var list = await _consultantService.GetConsultant();
                var consultantDto = list.Find(p => p.FamilyName == command.ConsultantDto.FamilyName);
                return consultantDto;
            }
        }
    }
}