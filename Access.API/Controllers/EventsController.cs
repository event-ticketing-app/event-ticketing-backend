using Access.API.Data;
using Access.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(EventCreateDto dto)
        {
            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                Date = dto.Date,
                Price = dto.Price
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, newEvent);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return eventItem;
        }

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

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteEvent(int id){

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
