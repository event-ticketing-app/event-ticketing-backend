using Access.API.Data;
using Access.API.Enums;
using Microsoft.EntityFrameworkCore;

namespace Access.API.Jobs
{
    public class ExpiredTicketsJob
    {
        private readonly AppDbContext _context;


        public ExpiredTicketsJob(AppDbContext context)
        {
            _context = context;
        }

        public async Task CancelExpiredTickets()
        {
            var expiredTickets = await _context.Tickets.Where(t => (t.Status == TicketStatus.Reserved && t.ExpiresAt < DateTime.UtcNow)).ToListAsync();

            foreach (var ticket in expiredTickets) {
                ticket.Status = TicketStatus.Cancelled;
            }
            await _context.SaveChangesAsync();
        }
    }
}
