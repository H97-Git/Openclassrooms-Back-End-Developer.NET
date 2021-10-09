using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Features.Commands.Availability;
using CalifornianHealth.Booking.Features.Queries.Availability;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalifornianHealth.Booking.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AvailabilityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Gets all availabilities.
        /// </summary>
        /// <returns>
        ///     The <see cref="List{T}" /> A list of availability.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<AvailabilityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AvailabilityDto>>> GetAllAsync()
        {
            var query = new GetAll.Query();
            var result = await _mediator.Send(query);
            return Ok(result.AvailabilityDto);
        }

        /// <summary>
        ///     Gets a availability by id.
        /// </summary>
        /// <param name="id">The id of a availability.</param>
        /// <returns>
        ///     The <see cref="AvailabilityDto" /> A availability information.
        /// </returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AvailabilityDto>> GetByIdAsync(int id)
        {
            var query = new Get.Query(id);
            var response = await _mediator.Send(query);
            return Ok(response.AvailabilityDto);
        }

        /// <summary>
        ///     Gets a availabilities by consultant id.
        /// </summary>
        /// <param name="consultantId">The id of a consultant.</param>
        /// <returns>
        ///     The <see cref="AvailabilityDto" /> A availability information.
        /// </returns>
        [HttpGet("Consultant/{consultantId:int}")]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AvailabilityDto>> GetByConsultantIdAsync(int consultantId)
        {
            var query = new GetByConsultantId.Query(consultantId);
            var response = await _mediator.Send(query);
            return Ok(response.AvailabilityDto);
        }

        /// <summary>
        ///     Save a availability.
        /// </summary>
        /// <param name="command"> The command with the availability information from the query</param>
        /// <returns>
        ///     The <see cref="ActionResult{AvailabilityDto}" /> The availability created.
        /// </returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<AvailabilityDto>> PostAsync([FromBody] PostAvailability.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction("GetById", new {result.Id}, result);
        }

        /// <summary>
        ///     Update a availability.
        /// </summary>
        /// <param name="availabilityDto">The date of the up to date availability.</param>
        /// <returns>
        ///     The <see cref="ActionResult{AvailabilityDto}" /> The availability updated.
        /// </returns>
        [HttpPut("edit")]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AvailabilityDto>> PutAsync([FromBody] AvailabilityDto availabilityDto)
        {
            var command = new PutAvailability.Command(availabilityDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        ///     Delete a availability.
        /// </summary>
        /// <param name="id">The id of a availability.</param>
        /// <returns>
        ///     The <see cref="ActionResult{AvailabilityDto}" /> The availability updated.
        /// </returns>
        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AvailabilityDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AvailabilityDto>> DeleteAsync(int id)
        {
            var command = new DeleteAvailability.Command(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}