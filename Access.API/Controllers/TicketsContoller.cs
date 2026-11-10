using Access.API.DTOs;
using Access.API.Enums;
using Access.API.Repositories;
using Access.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);


        [Authorize]
        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve([FromBody] TicketReserveDto dto)
        {
            var reservedTicket = await _ticketService.ReserveTicket( dto.EventId, CurrentUserId);

            return CreatedAtAction(nameof(Reserve), reservedTicket);
        }

        /// <summary>Starts the Stripe payment for a reserved ticket. Returns the client secret used by Stripe.js.</summary>
        [Authorize]
        [HttpPost("{id}/payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent(int id)
        {
            var clientSecret = await _ticketService.CreatePaymentIntent(id, CurrentUserId);

            return Ok(new { clientSecret });
        }

        [Authorize]
        [HttpPost("purchase/{id}")]
        public async Task<IActionResult> Purchase(int id, [FromBody] TicketPurchaseRequestDto? dto)
        {
            var purchasedTicket = await _ticketService.PurchaseTicket(id, CurrentUserId, dto?.PaymentIntentId);

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
            if (ticketItem.UserId != CurrentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return TicketMapper.ToPurchaseDto(ticketItem);
        }

        [Authorize]
        [HttpGet("my-tickets")]
        public async Task<ActionResult<IEnumerable<TicketPurchaseResponseDto>>> GetTickets()
        {
            var tickets = await _ticketRepository.GetTicketsByUserIdAsync(CurrentUserId);

            return Ok(tickets.Select(TicketMapper.ToPurchaseDto));
        }

        [Authorize]
        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var ticket = await _ticketRepository.GetByIdWithDetailsAsync(id);

            if (ticket == null) return NotFound();
            if (ticket.UserId != CurrentUserId) return Forbid();
            if (ticket.Status != TicketStatus.Reserved) return BadRequest("Ticket cannot be cancelled");
            ticket.Status = TicketStatus.Cancelled;

            await _ticketRepository.SaveAsync();
            return NoContent();
        }
    }

}
