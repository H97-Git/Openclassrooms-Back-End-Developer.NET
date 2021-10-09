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
    public class TradeController : BaseApiController
    {
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        // GET: api/Trades
        /// <summary>
        /// Gets all the trades.
        /// </summary>
        /// <returns>
        ///The <see cref="ActionResult"/>A list of trades.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(TradeDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IList<TradeDto>> GetTrade()
        {
            return await _tradeService.GetTrade();
        }

        // GET: api/Trades/5
        /// <summary>
        /// Gets a trade by Id.
        /// </summary>
        /// <param name="id">The Id of the trade.</param>
        /// <returns>
        ///The <see cref="ActionResult"/>A trade.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TradeDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TradeDto>> GetTrade(int id)
        {
            try
            {
                var trade = await _tradeService.GetTrade(id);
                return trade;
            }
            catch (Exception ex)
            {
                return HandleException(id,ex);
            }
        }

        // PUT: api/Trades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Update a trade.
        ///  </summary>
        ///  <param name="id">The Id of the current trade.</param>
        ///  <param name="tradeDto">The trade with the new value.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A trade.
        ///  </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TradeDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PutTrade(int id,TradeDto tradeDto)
        {
            if (id != tradeDto.Id)
            {
                tradeDto.Id = id;
            }

            try
            {
                await _tradeService.UpdateTrade(GetUserClaims(),tradeDto);
            }
            catch (Exception ex)
            {
                return HandleException(id,ex);
            }

            return Ok(_tradeService.GetTrade(id));
        }

        // POST: api/Trades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Save a trade.
        ///  </summary>
        ///  <param name="tradeDto">The trade to save.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A trade.
        ///  </returns>
        [HttpPost]
        [ProducesResponseType(typeof(TradeDto),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<TradeDto> PostTrade(TradeDto tradeDto)
        {
            _tradeService.SaveTrade(GetUserClaims(),tradeDto);

            return CreatedAtAction("GetTrade",new { id = tradeDto.Id },tradeDto);
        }

        // DELETE: api/Trades/5
        ///  <summary>
        ///  Delete a trade.
        ///  </summary>
        ///  <param name="id">The Id of the trade to delete.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/> Nothing.
        ///  </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(TradeDto),StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTrade(int id)
        {
            try
            {
                await _tradeService.DeleteTrade(GetUserClaims(),id);
            }
            catch (Exception ex)
            {
                return HandleException(id,ex);
            }
            return NoContent();
        }
    }
}