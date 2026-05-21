using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Access.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Access.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService=ticketService;
        }


        [Authorize]
        [HttpPost("reserve")]

        public async Task<IActionResult> Reserve(int eventId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var reservedTicket = await _ticketService.ReserveTicket( eventId, userId);

            return CreatedAtAction(nameof(Reserve), reservedTicket);

        }

        [Authorize]
        [HttpPost("purchase/{id}")]                
        public async Task<IActionResult> Purchase(int id)
        {
            var purchasedTicket = await _ticketService.PurchaseTicket(id);

            return Ok(purchasedTicket);
        }
    }

}