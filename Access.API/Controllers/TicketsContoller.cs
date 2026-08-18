using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Access.API.Services;
using Microsoft.AspNetCore.Mvc;
using Access.API.DTOs;
using Access.API.Repositories;

namespace Access.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ITicketRepository _ticketRepository;

        public TicketsController(ITicketService ticketService, ITicketRepository ticketRepository)
        {
            _ticketService=ticketService;
            _ticketRepository = ticketRepository;
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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketPurchaseResponseDto>> GetTicket(int id)
        {
            var ticketItem = await _ticketRepository.GetByIdWithDetailsAsync(id);

            if (ticketItem == null)
            {
                return NotFound();
            }

            var responseDto = new TicketPurchaseResponseDto
            {
                EventName = ticketItem.Event.Name,
                EventDescription = ticketItem.Event.Description,
                UserName = ticketItem.User.Name,
                Status = ticketItem.Status,
                Price = ticketItem.Price,
                EventDate = ticketItem.Event.Date,
                PurchaseAt = DateTime.UtcNow

            };
            return responseDto;
        }

    }

}