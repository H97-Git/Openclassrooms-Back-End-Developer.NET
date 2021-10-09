using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data.DTO;
using CalifornianHealth.Demographics.Features.Commands.Consultant;
using CalifornianHealth.Demographics.Features.Queries.Consultant;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalifornianHealth.Demographics.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ConsultantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConsultantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Gets all consultants.
        /// </summary>
        /// <returns>
        ///     The <see cref="List{T}" /> A list of consultant.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ConsultantDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ConsultantDto>>> GetAllAsync()
        {
            var query = new GetAllConsultant.Query();
            var result = await _mediator.Send(query);
            return Ok(result.ConsultantDto);
        }

        /// <summary>
        ///     Gets a consultant by id.
        /// </summary>
        /// <param name="id">The id of a consultant.</param>
        /// <returns>
        ///     The <see cref="ConsultantDto" /> A consultant information.
        /// </returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ConsultantDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ConsultantDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ConsultantDto>> GetByIdAsync(int id)
        {
            var query = new GetConsultant.Query(id);
            var response = await _mediator.Send(query);
            return Ok(response.ConsultantDto);
        }

        /// <summary>
        ///     Save a consultant.
        /// </summary>
        /// <param name="command"> The command with the consultant information from the query</param>
        /// <returns>
        ///     The <see cref="ActionResult{ConsultantDto}" /> The consultant created.
        /// </returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ConsultantDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<ConsultantDto>> PostAsync([FromBody] PostConsultant.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction("GetById", new {result.Id}, result);
        }

        /// <summary>
        ///     Update a consultant.
        /// </summary>
        /// <param name="consultantDto">The date of the up to date consultant.</param>
        /// <returns>
        ///     The <see cref="ActionResult{ConsultantDto}" /> The consultant updated.
        /// </returns>
        [HttpPut("edit")]
        [ProducesResponseType(typeof(ConsultantDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ConsultantDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ConsultantDto>> PutAsync([FromBody] ConsultantDto consultantDto)
        {
            var command = new PutConsultant.Command(consultantDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        ///     Delete a consultant.
        /// </summary>
        /// <param name="id">The id of a consultant.</param>
        /// <returns>
        ///     The <see cref="ActionResult{ConsultantDto}" /> The consultant updated.
        /// </returns>
        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(typeof(ConsultantDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ConsultantDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ConsultantDto>> DeleteAsync(int id)
        {
            var command = new DeleteConsultant.Command(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}