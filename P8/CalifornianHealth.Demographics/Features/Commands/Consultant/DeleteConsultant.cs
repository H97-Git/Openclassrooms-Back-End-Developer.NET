using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Demographics.Features.Commands.Consultant
{
    public static class DeleteConsultant
    {
        public record Command(int Id) : IRequest<int>;

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IConsultantService _consultantService;

            public Handler(IConsultantService consultantService)
            {
                _consultantService = consultantService;
            }

            public async Task<int> Handle(Command command, CancellationToken cancellationToken)
            {
                await _consultantService.DeleteConsultant(command.Id);
                return command.Id;
            }
        }
    }
}