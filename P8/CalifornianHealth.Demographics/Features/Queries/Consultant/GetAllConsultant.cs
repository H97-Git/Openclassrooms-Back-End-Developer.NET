using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data.DTO;
using CalifornianHealth.Demographics.Infrastructure.Services.Interface;
using MediatR;

namespace CalifornianHealth.Demographics.Features.Queries.Consultant
{
    public static class GetAllConsultant
    {
        public record Query : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IConsultantService _consultantService;

            public Handler(IConsultantService consultantService)
            {
                _consultantService = consultantService;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                return new Response(await _consultantService.GetConsultant());
            }
        }

        public record Response(List<ConsultantDto> ConsultantDto);
    }
}