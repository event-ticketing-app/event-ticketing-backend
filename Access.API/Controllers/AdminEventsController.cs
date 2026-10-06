using Access.API.DTOs;
using Access.API.Models;
using Access.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Access.API.Controllers
{
    [Route("api/admin/events")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminEventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public AdminEventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminEventResponseDto>>> GetEvents()
        {
            var events = await _eventRepository.GetAllAsync();
            return Ok(events.Select(e => new AdminEventResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Price = e.Price,
                TicketCapacity = e.TicketCapacity,
                ImageUrl = e.ImageUrl,
                OrganizerName = e.Organizer?.Name ?? string.Empty,
                OrganizerEmail = e.Organizer?.Email ?? string.Empty
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminEventResponseDto>> GetEvent(int id)
        {
            var e = await _eventRepository.GetByIdAsync(id);
            if (e == null) return NotFound();

            return Ok(new AdminEventResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Price = e.Price,
                TicketCapacity = e.TicketCapacity,
                ImageUrl = e.ImageUrl,
                OrganizerName = e.Organizer?.Name ?? string.Empty,
                OrganizerEmail = e.Organizer?.Email ?? string.Empty
            });
        }

        [HttpPost]
        public async Task<ActionResult<AdminEventResponseDto>> PostEvent(AdminEventCreateDto dto)
        {
            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                Date = dto.Date,
                Price = dto.Price,
                TicketCapacity = dto.TicketCapacity,
                ImageUrl = dto.ImageUrl,
                OrganizerId = dto.OrganizerId
            };

            await _eventRepository.AddAsync(newEvent);

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, new AdminEventResponseDto
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Description = newEvent.Description,
                Date = newEvent.Date,
                Price = newEvent.Price,
                TicketCapacity = newEvent.TicketCapacity,
                ImageUrl = newEvent.ImageUrl
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventUpdateDto dto)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null) return NotFound();

            eventItem.Name = dto.Name;
            eventItem.Description = dto.Description;
            eventItem.Date = dto.Date;
            eventItem.Price = dto.Price;
            eventItem.TicketCapacity = dto.TicketCapacity;
            eventItem.ImageUrl = dto.ImageUrl;

            await _eventRepository.UpdateAsync(id, eventItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null) return NotFound();

            await _eventRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}