using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Data;
using RestAPI.Models.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RegisterValidator _registerValidator;

        public AuthenticateController(UserManager<User> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _registerValidator = new RegisterValidator();
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user,model.Password))
            {
                return Unauthorized();
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("UserId",user.Id)
            };

            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role,userRole)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
            );
            return Ok(new
            {
                role = userRoles,
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var validationResult = await _registerValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest,new Response { StatusCode = "Validation Error",Message = validationResult.ToString() });
            }

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { StatusCode = "Error",Message = "User Name already taken!" });
            }

            var user = new User()
            {
                Fullname = model.FullName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Role = "User"
            };

            var result = await _userManager.CreateAsync(user,model.Password);
            await _userManager.AddToRoleAsync(user,"User");

            return result.Succeeded
                ? Ok(new Response { StatusCode = "Success",Message = "User created successfully!" })
                : StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        StatusCode = "Error",
                        Message = result.ToString()
                    });
        }
    }
}