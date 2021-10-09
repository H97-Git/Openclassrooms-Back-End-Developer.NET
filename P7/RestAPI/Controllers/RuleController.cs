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
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController : BaseApiController
    {
        private readonly IRuleService _ruleService;

        public RuleController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        // GET: api/Rules
        /// <summary>
        /// Gets all the rules.
        /// </summary>
        /// <returns>
        ///The <see cref="ActionResult"/>A list of rules.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(RuleDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IList<RuleDto>> GetRule()
        {
            return await _ruleService.GetRule();
        }

        // GET: api/Rules/5
        /// <summary>
        /// Gets a rule by Id.
        /// </summary>
        /// <param name="id">The Id of the rule.</param>
        /// <returns>
        ///The <see cref="ActionResult"/>A rule.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RuleDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RuleDto>> GetRule(int id)
        {
            try
            {
                var rule = await _ruleService.GetRule(id);
                return rule;
            }
            catch (Exception ex)
            {
                return HandleException(id,ex);
            }
        }

        // PUT: api/Rules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Update a rule.
        ///  </summary>
        ///  <param name="id">The Id of the current rule.</param>
        ///  <param name="ruleDto">The rule with the new value.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A rule.
        ///  </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RuleDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PutRule(int id,RuleDto ruleDto)
        {
            if (id != ruleDto.Id)
            {
                ruleDto.Id = id;
            }

            try
            {
                await _ruleService.UpdateRule(ruleDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RuleExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return Ok(_ruleService.GetRule(id));
        }

        // POST: api/Rules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Save a rule.
        ///  </summary>
        ///  <param name="ruleDto">The rule to save.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A rule.
        ///  </returns>
        [HttpPost]
        [ProducesResponseType(typeof(RuleDto),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<RuleDto> PostRule(RuleDto ruleDto)
        {
            _ruleService.SaveRule(ruleDto);

            return CreatedAtAction("GetRule",new { id = ruleDto.Id },ruleDto);
        }

        // DELETE: api/Rules/5
        ///  <summary>
        ///  Delete a rule.
        ///  </summary>
        ///  <param name="id">The Id of the rule to delete.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/> Nothing.
        ///  </returns>
        /// <response code="404">If the rule is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(RuleDto),StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRule(int id)
        {
            var ruleDto = await _ruleService.GetRule(id);
            if (ruleDto == null)
            {
                return NotFound();
            }

            await _ruleService.DeleteRule(id);

            return NoContent();
        }

        private async Task<bool> RuleExists(int id)
        {
            return await _ruleService.GetRule(id) != null;
        }
    }
}