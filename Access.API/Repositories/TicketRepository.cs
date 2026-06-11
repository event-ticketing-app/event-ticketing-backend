using Access.API.Data;
using Access.API.Models;
using Access.API.Enums;
using Microsoft.EntityFrameworkCore;

namespace Access.API.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository (AppDbContext context)
        {
            _context = context;

        }

        public async Task<int> CountActiveTicketsAsync(int eventId)
        {
            var ticketocuppied = await _context.Tickets.Where(t => (t.Status == TicketStatus.Purchased || t.Status == TicketStatus.Reserved) && t.EventId == eventId).CountAsync();

            return ticketocuppied;
        }

        public async Task<bool> HasActiveReservationAsync(int eventId, int userId)
        {
            var ticketuser = await _context.Tickets.Where(t => t.EventId == eventId && t.UserId == userId && t.Status == TicketStatus.Reserved).AnyAsync();

            return ticketuser;
        }

        public async Task<Ticket> AddAsync(Ticket ticket, Event eventItem)
        {
            _context.Tickets.Add(ticket);
            _context.Entry(eventItem).State = EntityState.Modified;

            return ticket;
        }

        public async Task<Ticket?> GetByIdWithDetailsAsync(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            return ticket;
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
