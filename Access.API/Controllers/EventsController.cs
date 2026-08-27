using Access.API.Data;
using Access.API.DTOs;
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

            return Ok(events.Select(e => new EventResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Price = e.Price,
                ImageUrl = e.ImageUrl,
                TicketCapacity = e.TicketCapacity

            }));
            
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPost]
        public async Task<ActionResult<EventResponseDto>> PostEvent(EventCreateDto dto)
        {
            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                Date = dto.Date,
                Price = dto.Price,
                TicketCapacity = dto.TicketCapacity
            };


            newEvent = await _eventRepository.AddAsync(newEvent);

            var responseDto = new EventResponseDto
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Description = newEvent.Description,
                Date = newEvent.Date,
                Price = newEvent.Price,
                TicketCapacity = newEvent.TicketCapacity

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

            var responseDto = new EventResponseDto
            {
                Id = eventItem.Id,
                Name = eventItem.Name,
                Description = eventItem.Description,
                Date = eventItem.Date,
                Price = eventItem.Price,
                ImageUrl = eventItem.ImageUrl,
                TicketCapacity = eventItem.TicketCapacity

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

    }
}
