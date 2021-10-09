using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected (string userId, string userRole) GetUserClaims()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            return (userId, userRole);
        }

        protected ActionResult HandleException(int id, Exception ex)
        {
            return ex.GetType().Name switch
            {
                "AuthenticationException" => StatusCode(StatusCodes.Status403Forbidden,
                    new Response
                    {
                        EntityId = id,
                        StatusCode = StatusCodes.Status403Forbidden.ToString(),
                        Message = ex.Message
                    }),
                "KeyNotFoundException" => StatusCode(StatusCodes.Status404NotFound,
                    new Response
                    {
                        EntityId = id,
                        StatusCode = StatusCodes.Status404NotFound.ToString(),
                        Message = ex.Message
                    }),
                _ => StatusCode(StatusCodes.Status501NotImplemented,
                    new Response
                    {
                        EntityId = id,
                        StatusCode = StatusCodes.Status501NotImplemented.ToString(),
                        Message = ex.Message
                    })
            };
        }
    }
}