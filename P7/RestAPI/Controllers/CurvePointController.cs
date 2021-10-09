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
    public class CurvePointController : BaseApiController
    {
        private readonly ICurvePointService _curvePointService;

        public CurvePointController(ICurvePointService curvePointService)
        {
            _curvePointService = curvePointService;
        }

        // GET: api/CurvePoints
        /// <summary>
        /// Gets all the curve-points.
        /// </summary>
        /// <returns>
        ///The <see cref="ActionResult"/>A list of curve-points.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(CurvePointDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IList<CurvePointDto>> GetCurvePoint()
        {
            return await _curvePointService.GetCurvePoint();
        }

        // GET: api/CurvePoints/5
        /// <summary>
        /// Gets a curve-point by Id.
        /// </summary>
        /// <param name="id">The Id of the curve-point.</param>
        /// <returns>
        ///The <see cref="ActionResult"/>A curve-point.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CurvePointDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CurvePointDto>> GetCurvePoint(int id)
        { 
            try
            {
                var curvePoint = await _curvePointService.GetCurvePoint(id);
                return curvePoint;
            }
            catch (Exception ex)
            {
                return HandleException(id,ex);
            }
        }

        // PUT: api/CurvePoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Update a curve-point.
        ///  </summary>
        ///  <param name="id">The Id of the current curve-point.</param>
        ///  <param name="curvePointDto">The curve-point with the new value.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A curve-point.
        ///  </returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CurvePointDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PutCurvePoint(int id,CurvePointDto curvePointDto)
        {
            if (id != curvePointDto.Id)
            {
                curvePointDto.Id = id;
            }

            try
            {
                await _curvePointService.UpdateCurvePoint(curvePointDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CurvePointExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return Ok(_curvePointService.GetCurvePoint(id));
        }

        // POST: api/CurvePoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Save a curve-point.
        ///  </summary>
        ///  <param name="curvePointDto">The curve-point to save.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A curve-point.
        ///  </returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(CurvePointDto),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<CurvePointDto> PostCurvePoint(CurvePointDto curvePointDto)
        {
            _curvePointService.SaveCurvePoint(curvePointDto);

            return CreatedAtAction("GetCurvePoint",new { id = curvePointDto.Id },curvePointDto);
        }

        // DELETE: api/CurvePoints/5
        ///  <summary>
        ///  Delete a curve-point.
        ///  </summary>
        ///  <param name="id">The Id of the curve-point to delete.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/> Nothing.
        ///  </returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCurvePoint(int id)
        {
            var curvePointDto = await _curvePointService.GetCurvePoint(id);
            if (curvePointDto == null)
            {
                return NotFound();
            }

            await _curvePointService.DeleteCurvePoint(id);

            return NoContent();
        }

        private async Task<bool> CurvePointExists(int id)
        {
            return await _curvePointService.GetCurvePoint(id) != null;
        }
    }
}