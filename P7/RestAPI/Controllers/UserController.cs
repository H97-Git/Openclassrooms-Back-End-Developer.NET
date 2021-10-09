using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Infrastructure.Services;
using RestAPI.Models.Authentication.DTO;

namespace RestAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        /// <summary>
        /// Gets all the users.
        /// </summary>
        /// <returns>
        ///The <see cref="ActionResult"/>A list of users.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IList<UserDto>> GetUser()
        {
            return await _userService.GetUser();
        }

        // GET: api/Users/5
        /// <summary>
        /// Gets a user by Id.
        /// </summary>
        /// <param name="id">The Id of the user.</param>
        /// <returns>
        ///The <see cref="ActionResult"/>A user.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var user = await _userService.GetUser(id);

            return user == null ? NotFound() : (ActionResult<UserDto>)user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Update a user.
        ///  </summary>
        ///  <param name="id">The Id of the current user.</param>
        ///  <param name="userDto">The user with the new value.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A user.
        ///  </returns> 
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUser(string id,UserDto user)
        {
            if (id != user.Id)
            {
                user.Id = id;
            }

            try
            {
                await _userService.UpdateUser(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return Ok(_userService.GetUser(id));
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ///  <summary>
        ///  Save a user.
        ///  </summary>
        ///  <param name="userDto">The user to save.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/>A user.
        ///  </returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<UserDto> PostUser(UserDto user)
        {
            _userService.SaveUser(user);

            return CreatedAtAction("GetUser",new { id = user.Id },user);
        }

        // DELETE: api/Users/5
        ///  <summary>
        ///  Delete a user.
        ///  </summary>
        ///  <param name="id">The Id of the user to delete.</param>
        ///  <returns>
        /// The <see cref="ActionResult"/> Nothing.
        ///  </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUser(id);

            return NoContent();
        }

        private async Task<bool> UserExists(string id)
        {
            return await _userService.GetUser(id) != null;
        }
    }
}