using Access.API.Data;
using Access.API.DTOs;
using Access.API.Enums;
using Access.API.Models;
using Access.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Access.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetEvents()
        {
            var events = await _eventRepository.GetAllAsync();
            var soldCounts = await _eventRepository.CountActiveTicketsByEventAsync();

            // Only approved events are public
            return Ok(events.Where(e => e.Status == EventStatus.Active).Select(e => new EventResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Price = e.Price,
                ImageUrl = e.ImageUrl,
                TicketCapacity = e.TicketCapacity,
                Status = e.Status,
                OrganizerId = e.OrganizerId,
                Location = e.Location,
                TicketsSold = soldCounts.GetValueOrDefault(e.Id)

            }));
            
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPost]
        public async Task<ActionResult<EventResponseDto>> PostEvent(EventCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var newEvent = new Event
            {
                OrganizerId = userId,
                Name = dto.Name,
                Description = dto.Description,
                Date = dto.Date,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                TicketCapacity = dto.TicketCapacity,
                Location = dto.Location,
                // Organizer events always need admin approval
                Status = EventStatus.Pending
            };


            newEvent = await _eventRepository.AddAsync(newEvent);

            var responseDto = new EventResponseDto
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Description = newEvent.Description,
                Date = newEvent.Date,
                Price = newEvent.Price,
                ImageUrl = newEvent.ImageUrl,
                TicketCapacity = newEvent.TicketCapacity,
                Status = newEvent.Status,
                OrganizerId = newEvent.OrganizerId,
                Location = newEvent.Location,
                TicketsSold = 0

            };
            return CreatedAtAction(nameof(GetEvent), new { id = responseDto.Id }, responseDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponseDto>> GetEvent(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            var soldCount = (await _eventRepository.CountActiveTicketsByEventAsync()).GetValueOrDefault(eventItem.Id);
            var responseDto = new EventResponseDto
            {
                Id = eventItem.Id,
                Name = eventItem.Name,
                Description = eventItem.Description,
                Date = eventItem.Date,
                Price = eventItem.Price,
                ImageUrl = eventItem.ImageUrl,
                TicketCapacity = eventItem.TicketCapacity,
                Status = eventItem.Status,
                OrganizerId = eventItem.OrganizerId,
                Location = eventItem.Location,
                TicketsSold = soldCount

            };
            return responseDto;
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var eventItem = await _eventRepository.GetByIdAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            if (eventItem.OrganizerId != userId && !User.IsInRole("Admin"))
            {

                return Forbid();
                

            }
            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                Date = dto.Date,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                TicketCapacity = dto.TicketCapacity,
                Location = dto.Location,
                // Any change made by an organizer must be approved again
                Status = EventStatus.Pending
            };

            await _eventRepository.UpdateAsync(id, newEvent);

            return NoContent();
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var eventItem = await _eventRepository.GetByIdAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            if (eventItem.OrganizerId != userId && !User.IsInRole("Admin"))
            {

                return Forbid();


            }

            await _eventRepository.DeleteAsync(id);



            return NoContent();

        }
        [Authorize]
        [HttpGet("my-events")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetEventsByUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var events = await _eventRepository.GetEventsByUserIdAsync(userId);
            var soldCounts = await _eventRepository.CountActiveTicketsByEventAsync();

            return Ok(events.Select(e => new EventResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Price = e.Price,
                ImageUrl = e.ImageUrl,
                TicketCapacity = e.TicketCapacity,
                Status = e.Status,
                OrganizerId = e.OrganizerId,
                Location = e.Location,
                TicketsSold = soldCounts.GetValueOrDefault(e.Id)
            }));
        }

    }
}
