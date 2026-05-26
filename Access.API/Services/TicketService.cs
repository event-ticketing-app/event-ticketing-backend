using Access.API.Data;
using Access.API.DTOs;
using Access.API.Models;
using Access.API.Enums;
using Microsoft.EntityFrameworkCore;

namespace Access.API.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;

        public TicketService (AppDbContext context)
        {
            _context = context;

        }
        public  async Task<TicketReserveResponseDto> ReserveTicket(int EventId, int UserId)
        {

            var event1 = await _context.Events.FindAsync(EventId);

            if (event1 == null)
            {
                throw new Exception("Event doesn't exists");
            }
            var ticketocuppied = await _context.Tickets.Where(t => (t.Status == TicketStatus.Purchased || t.Status == TicketStatus.Reserved) && t.EventId == EventId).CountAsync();

            if (ticketocuppied >= event1.TicketCapacity)
            {
                throw new Exception("Event is full");

            }

            var ticketuser = await _context.Tickets.Where(t => t.EventId == EventId && t.UserId == UserId && t.Status == TicketStatus.Reserved).AnyAsync();
            
            if (ticketuser)
            {
                throw new Exception("You have an active reserve");
            }
            

            var newTicket = new Ticket
            {   EventId = EventId,
                UserId = UserId,
                Price = event1.Price,
                Status = TicketStatus.Reserved,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)

            };

            _context.Tickets.Add(newTicket);
            _context.Entry(event1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Event is full");
            }

            var responseDTO = new TicketReserveResponseDto
            {
                Status =  newTicket.Status,
                Price =  newTicket.Price,
                ExpiresAt = newTicket.ExpiresAt

            };

            return responseDTO;

        }

        public async Task<TicketPurchaseResponseDto> PurchaseTicket(int Id)
        {
            var ticket =  await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == Id);

            if (ticket == null)
            {
                throw new Exception("Your ticket is not available");
            }

            if (ticket.Status != TicketStatus.Reserved)
            {
                throw new Exception("Ticket is not in reserved status");
            }

            if (ticket.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Ticket reservation has expired");
            }

            ticket.Status = TicketStatus.Purchased;

            await _context.SaveChangesAsync();

            var responseDTO =  new TicketPurchaseResponseDto
            {
                EventName = ticket.Event.Name,
                EventDescription = ticket.Event.Description,
                UserName = ticket.User.Name,
                Status = ticket.Status,
                Price = ticket.Price,
                EventDate = ticket.Event.Date,
                PurchaseAt = DateTime.UtcNow


            };

            return responseDTO;
        }

    }
}