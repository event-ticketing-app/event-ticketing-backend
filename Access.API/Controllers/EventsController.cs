using Access.API.Data;
using Access.API.Models;
using Access.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Access.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetEvents()
        {
            var events = await _context.Events.ToListAsync();

            return Ok(events.Select(e => new EventResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Price = e.Price

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


            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

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
            var eventItem = await _context.Events.FindAsync(id);

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
                Price = eventItem.Price

            };
            return responseDto;
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventCreateDto dto)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            eventItem.Name = dto.Name;
            eventItem.Description = dto.Description;
            eventItem.Date = dto.Date;
            eventItem.Price = dto.Price;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Organizer,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null){

                return NotFound();
            }
            _context.Events.Remove(eventItem);
            
            await _context.SaveChangesAsync();


            return NoContent();

        }

    }
}
