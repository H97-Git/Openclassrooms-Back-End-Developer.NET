using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data.DTO;
using CalifornianHealth.Demographics.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Demographics.Features.Commands.Patient
{
    public static class PutPatient
    {
        public record Command(PatientDto PatientDto) : IRequest<PatientDto>;

        public class Handler : IRequestHandler<Command, PatientDto>
        {
            private readonly IPatientService _patientService;

            public Handler(IPatientService patientService)
            {
                _patientService = patientService;
            }

            public async Task<PatientDto> Handle(Command command, CancellationToken cancellationToken)
            {
                await _patientService.UpdatePatient(command.PatientDto);
                return await _patientService.GetPatient(command.PatientDto.Id);
            }
        }
    }
}