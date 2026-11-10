using Access.API.Data;
using Access.API.Enums;
using Access.API.Services;
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
            // Leave a short grace period so a payment that finished right at the end can still be confirmed
            var cutoff = DateTime.UtcNow.AddMinutes(-TicketRules.PaymentGraceMinutes);
            var expiredTickets = await _context.Tickets.Where(t => (t.Status == TicketStatus.Reserved && t.ExpiresAt < cutoff)).ToListAsync();

            foreach (var ticket in expiredTickets) {
                ticket.Status = TicketStatus.Cancelled;
            }
            await _context.SaveChangesAsync();
        }
    }
}
