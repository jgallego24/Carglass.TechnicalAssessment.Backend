using Microsoft.AspNetCore.Mvc;

namespace Carglass.TechnicalAssessment.Backend.Api.Controllers
{
    [ApiController]
    [Route("keepalive")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult KeepAlive()
        {
            return Ok();
        }
    }
}
