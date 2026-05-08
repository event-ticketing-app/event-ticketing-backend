

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

            if (ticketocuppied >= event1.ticketcapacity)
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
            await _context.SaveChangesAsync();

            var responseDTO = new TicketReserveResponseDto
            {
                Status =  newTicket.Status,
                Price =  newTicket.Price,
                ExpiresAt = newTicket.ExpiresAt

            };

            return responseDTO;

        }

        public TicketPurchaseDto PurchaseTicket(int Id)
        {
            

        }

    }
}