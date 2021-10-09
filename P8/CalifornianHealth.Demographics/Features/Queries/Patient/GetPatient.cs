using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data.DTO;
using CalifornianHealth.Demographics.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Demographics.Features.Queries.Patient
{
    public static class GetPatient
    {
        public record Query(int Id) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IPatientService _patientService;

            public Handler(IPatientService patientService)
            {
                _patientService = patientService;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                return new Response(await _patientService.GetPatient(query.Id));
            }
        }

        public record Response(PatientDto PatientDto);
    }
}