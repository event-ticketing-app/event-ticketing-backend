using Access.API.Data;
using Access.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Access.API.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var events = await _context.Events.ToListAsync();

            return events;
        }

        public async Task<Event?> GetByIdAsync(int id) 
        {
            var eventItem = await _context.Events.FindAsync(id);

            return eventItem;
        }

        public async Task<Event> AddAsync(Event newEvent)
        {
            _context.Events.Add(newEvent);

            await _context.SaveChangesAsync();

            return newEvent;
        }

        public async Task UpdateAsync(int id, Event newEvent)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)  
            {
                return;
            }

            eventItem.Name = newEvent.Name;
            eventItem.Description = newEvent.Description;
            eventItem.Date = newEvent.Date;
            eventItem.Price = newEvent.Price;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return;
            }

            _context.Events.Remove(eventItem);

            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId)
        {

            return await _context.Events
                .Where(t => t.OrganizerId == userId)
                .ToListAsync();
        }

    }
}

