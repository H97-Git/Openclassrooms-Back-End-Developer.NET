using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Demographics.Data.DTO;
using CalifornianHealth.Demographics.Features.Commands.Patient;
using CalifornianHealth.Demographics.Features.Queries.Patient;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalifornianHealth.Demographics.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Gets all patients.
        /// </summary>
        /// <returns>
        ///     The <see cref="List{T}" /> A list of patient.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<PatientDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PatientDto>>> GetAllAsync()
        {
            var query = new GetAllPatient.Query();
            var result = await _mediator.Send(query);
            return Ok(result.PatientDto);
        }

        /// <summary>
        ///     Gets a patient by id.
        /// </summary>
        /// <param name="id">The id of a patient.</param>
        /// <returns>
        ///     The <see cref="PatientDto" /> A patient information.
        /// </returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> GetByIdAsync(int id)
        {
            var query = new GetPatient.Query(id);
            var response = await _mediator.Send(query);
            return Ok(response.PatientDto);
        }

        /// <summary>
        ///     Save a patient.
        /// </summary>
        /// <param name="command"> The command with the patient information from the query</param>
        /// <returns>
        ///     The <see cref="ActionResult{PatientDto}" /> The patient created.
        /// </returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<PatientDto>> PostAsync([FromBody] PostPatient.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction("GetById", result.Id, result);
        }

        /// <summary>
        ///     Update a patient.
        /// </summary>
        /// <param name="patientDto">The date of the up to date patient.</param>
        /// <returns>
        ///     The <see cref="ActionResult{PatientDto}" /> The patient updated.
        /// </returns>
        [HttpPut("edit")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> PutAsync([FromBody] PatientDto patientDto)
        {
            var command = new PutPatient.Command(patientDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        ///     Delete a patient.
        /// </summary>
        /// <param name="id">The id of a patient.</param>
        /// <returns>
        ///     The <see cref="ActionResult{PatientDto}" /> The patient updated.
        /// </returns>
        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PatientDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDto>> DeleteAsync(int id)
        {
            var command = new DeletePatient.Command(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}