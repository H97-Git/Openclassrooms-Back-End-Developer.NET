using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Infrastructure.Services;
using RestAPI.Models.DTO;

namespace RestAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : BaseApiController
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        // GET: api/Bids
        /// <summary>
        /// Gets all the bids.
        /// </summary>
        /// <returns>
        ///The <see cref="ActionResult"/>A list of bids.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BidDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IList<BidDto>> GetBid()
        {
            return await _bidService.GetBid();
        }

        // GET: api/Bids/5
        /// <summary>
        /// Gets a bid by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The Id of the bid.</param>
        /// <returns>
        ///The <see cref="ActionResult"/>A bid.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BidDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BidDto>> GetBid(int id)
        {
            try
            {
                var bid = await _bidService.GetBid(id);
                return bid;
            }
            catch (Exception ex)
            {
                return HandleException(id, ex);
            }
        }

        // PUT: api/Bids/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Update a bid.
        ///  </summary>
        ///  <param name="id">The Id of the current bid.</param>
        ///  <param name="bidDto">The bid with the new value.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A bid.
        ///  </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BidDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PutBid(int id, BidDto bidDto)
        {
            if (id != bidDto.Id)
            {
                bidDto.Id = id;
            }

            try
            {
                await _bidService.UpdateBid(GetUserClaims(), bidDto);
            }
            catch (Exception ex)
            {
                return HandleException(id, ex);
            }

            return Ok(_bidService.GetBid(id));
        }

        // POST: api/Bids
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Save a bid.
        ///  </summary>
        ///  <param name="bidDto">The bid to save.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A bid.
        ///  </returns>
        [HttpPost]
        [ProducesResponseType(typeof(BidDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<BidDto> PostBid(BidDto bidDto)
        {
            _bidService.SaveBid(GetUserClaims(), bidDto);

            return CreatedAtAction("GetBid", new { id = bidDto.Id }, bidDto);
        }

        // DELETE: api/Bids/5
        ///  <summary>
        ///  Delete a bid.
        ///  </summary>
        ///  <param name="id">The Id of the bid to delete.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/> Nothing.
        ///  </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBid(int id)
        {
            try
            {
                await _bidService.DeleteBid(GetUserClaims(), id);
            }
            catch (Exception ex)
            {
                return HandleException(id, ex);
            }
            return NoContent();
        }
    }
}