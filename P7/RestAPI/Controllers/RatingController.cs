using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Infrastructure.Services;
using RestAPI.Models.DTO;

namespace RestAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : BaseApiController
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // GET: api/Ratings
        /// <summary>
        /// Gets all the ratings.
        /// </summary>
        /// <returns>
        ///The <see cref="ActionResult"/>A list of ratings.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(RatingDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IList<RatingDto>> GetRating()
        {
            return await _ratingService.GetRating();
        }

        // GET: api/Ratings/5
        /// <summary>
        /// Gets a rating by Id.
        /// </summary>
        /// <param name="id">The Id of the rating.</param>
        /// <returns>
        ///The <see cref="ActionResult"/>A rating.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RatingDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RatingDto>> GetRating(int id)
        {
            try
            {
                var rating = await _ratingService.GetRating(id);
                return rating;
            }
            catch (Exception ex)
            {
                return HandleException(id,ex);
            }
        }

        // PUT: api/Ratings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Update a rating.
        ///  </summary>
        ///  <param name="id">The Id of the current rating.</param>
        ///  <param name="ratingDto">The rating with the new value.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A rating.
        ///  </returns> 
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RatingDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PutRating(int id,RatingDto ratingDto)
        {
            if (id != ratingDto.Id)
            {
                ratingDto.Id = id;
            }

            try
            {
                await _ratingService.UpdateRating(ratingDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RatingExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return Ok(_ratingService.GetRating(id));
        }

        // POST: api/Ratings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Save a rating.
        ///  </summary>
        ///  <param name="ratingDto">The rating to save.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A rating.
        ///  </returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(RatingDto),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<RatingDto> PostRating(RatingDto ratingDto)
        {
            _ratingService.SaveRating(ratingDto);

            return CreatedAtAction("GetRating",new { id = ratingDto.Id },ratingDto);
        }

        // DELETE: api/Ratings/5
        ///  <summary>
        ///  Delete a rating.
        ///  </summary>
        ///  <param name="id">The Id of the rating to delete.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/> Nothing.
        ///  </returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(RatingDto),StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRating(int id)
        {
            var ratingDto = await _ratingService.GetRating(id);
            if (ratingDto == null)
            {
                return NotFound();
            }

            await _ratingService.DeleteRating(id);

            return NoContent();
        }

        private async Task<bool> RatingExists(int id)
        {
            return await _ratingService.GetRating(id) != null;
        }
    }
}