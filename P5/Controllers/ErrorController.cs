using Microsoft.AspNetCore.Mvc;

namespace The_Car_Hub.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("500")]
        public IActionResult AppError()
        {
            return View();
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}