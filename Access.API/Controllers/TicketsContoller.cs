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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ticketItem = await _ticketRepository.GetByIdWithDetailsAsync(id);

            if (ticketItem == null)
            {
                return NotFound();
            }
            if (ticketItem.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            var responseDto = new TicketPurchaseResponseDto
            {
                Id = ticketItem.Id,
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
        [Authorize]
        [HttpGet("my-tickets")]
        public async Task<ActionResult<IEnumerable<TicketPurchaseResponseDto>>> GetTickets()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var tickets = await _ticketRepository.GetTicketsByUserIdAsync(userId);

            return Ok(tickets.Select(e => new TicketPurchaseResponseDto
            {
                Id = e.Id,
                EventName = e.Event.Name,
                EventDescription = e.Event.Description,
                UserName = e.User.Name,
                Status = e.Status,
                Price = e.Price,
                EventDate = e.Event.Date,
                PurchaseAt = DateTime.UtcNow
            }));
        }


    }

}