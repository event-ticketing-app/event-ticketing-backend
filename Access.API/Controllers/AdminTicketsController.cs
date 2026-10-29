using Access.API.Enums;
using Access.API.Models;
using Access.API.Repositories;
using Access.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Access.API.Controllers
{
    [Route("api/admin/tickets")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminTicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public AdminTicketsController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminTicketResponseDto>>> GetTickets()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            var result = tickets.Select(ticket => new AdminTicketResponseDto
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                UserId = ticket.UserId,
                EventName = ticket.Event?.Name ?? string.Empty,
                EventDescription = ticket.Event?.Description ?? string.Empty,
                UserName = ticket.User?.Name ?? string.Empty,
                Status = ticket.Status,
                            Price = ticket.Price,
                            EventDate = ticket.Event?.Date ?? default,
                            PurchaseDate = ticket.PurchaseDate
                        });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminTicketResponseDto>> GetTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIdWithDetailsAsync(id);
            if (ticket == null) return NotFound();
            var dto = new AdminTicketResponseDto
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                UserId = ticket.UserId,
                EventName = ticket.Event?.Name ?? string.Empty,
                EventDescription = ticket.Event?.Description ?? string.Empty,
                UserName = ticket.User?.Name ?? string.Empty,
                Status = ticket.Status,
                            Price = ticket.Price,
                            EventDate = ticket.Event?.Date ?? default,
                            PurchaseDate = ticket.PurchaseDate
                        };
            return Ok(dto);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIdWithDetailsAsync(id);
            if (ticket == null) return NotFound();
            ticket.Status = TicketStatus.Cancelled;
            await _ticketRepository.UpdateAsync(ticket);
            await _ticketRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIdWithDetailsAsync(id);
            if (ticket == null) return NotFound();
            await _ticketRepository.DeleteAsync(id);
            await _ticketRepository.SaveAsync();
            return NoContent();
        }
    }
}
