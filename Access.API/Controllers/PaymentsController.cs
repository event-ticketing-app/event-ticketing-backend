using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Access.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PaymentsController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Stripe publishable key for the frontend. It is public by design, but served from here
        /// so both Stripe keys stay in the git-ignored appsettings.Development.json.
        /// </summary>
        [Authorize]
        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            var publishableKey = _config["Stripe:PublishableKey"];

            if (string.IsNullOrWhiteSpace(publishableKey) || !publishableKey.StartsWith("pk_"))
            {
                return StatusCode(503, new { message = "Payments are not configured yet (missing Stripe publishable key)." });
            }

            return Ok(new { publishableKey });
        }
    }
}
