using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Demographics.Features.Commands.Patient
{
    public static class DeletePatient
    {
        public record Command(int Id) : IRequest<int>;

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IPatientService _patientService;

            public Handler(IPatientService patientService)
            {
                _patientService = patientService;
            }

            public async Task<int> Handle(Command command, CancellationToken cancellationToken)
            {
                await _patientService.DeletePatient(command.Id);
                return command.Id;
            }
        }
    }
}