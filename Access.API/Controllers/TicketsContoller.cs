using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Access.API.Services;
using Microsoft.AspNetCore.Mvc;
using Access.API.DTOs;

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

        public async Task<IActionResult> Reserve([FromBody] TicketReserveDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var reservedTicket = await _ticketService.ReserveTicket( dto.EventId, userId);

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