using System.Collections.Generic;
using System.Threading.Tasks;
using CalifornianHealth.Booking.Data.DTO;
using CalifornianHealth.Booking.Features.Commands.Booking;
using CalifornianHealth.Booking.Features.Queries.Booking;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalifornianHealth.Booking.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Gets all bookings.
        /// </summary>
        /// <returns>
        ///     The <see cref="List{T}" /> A list of booking.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<BookingDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BookingDto>>> GetAllAsync()
        {
            var query = new GetAll.Query();
            var result = await _mediator.Send(query);
            return Ok(result.BookingDto);
        }

        /// <summary>
        ///     Gets a booking by id.
        /// </summary>
        /// <param name="id">The id of a booking.</param>
        /// <returns>
        ///     The <see cref="BookingDto" /> A booking information.
        /// </returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> GetByIdAsync(int id)
        {
            var query = new Get.Query(id);
            var response = await _mediator.Send(query);
            return Ok(response.BookingDto);
        }

        /// <summary>
        ///     Gets all bookings for a certain consultant.
        /// </summary>
        /// <param name="consultantId">The id of a consultant.</param>
        /// <returns>
        ///     The <see cref="List{T}" /> A booking information.
        /// </returns>
        [HttpGet("Consultant/{consultantId:int}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingDto>>> GetByConsultantIdAsync(int consultantId)
        {
            var query = new GetByConsultantId.Query(consultantId);
            var response = await _mediator.Send(query);
            return Ok(response.BookingDto);
        }

        /// <summary>
        ///     Save a booking.
        /// </summary>
        /// <param name="command"> The command with the booking information from the query</param>
        /// <returns>
        ///     The <see cref="ActionResult{BookingDto}" /> The booking created.
        /// </returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<BookingDto>> PostAsync([FromBody] PostBooking.Command command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction("GetById", new {result.Id}, result);
        }

        /// <summary>
        ///     Update a booking.
        /// </summary>
        /// <param name="bookingDto">The date of the up to date booking.</param>
        /// <returns>
        ///     The <see cref="ActionResult{BookingDto}" /> The booking updated.
        /// </returns>
        [HttpPut("edit")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> PutAsync([FromBody] BookingDto bookingDto)
        {
            var command = new PutBooking.Command(bookingDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        ///     Delete a booking.
        /// </summary>
        /// <param name="id">The id of a booking.</param>
        /// <returns>
        ///     The <see cref="ActionResult{BookingDto}" /> The booking updated.
        /// </returns>
        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> DeleteAsync(int id)
        {
            var command = new DeleteBooking.Command(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}