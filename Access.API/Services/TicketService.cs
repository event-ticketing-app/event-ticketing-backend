

namespace Access.API.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;

        public TicketService (AppDbContext context)
        {
            _context = context;

        }
        public  TicketReserveResponseDto ReserveTicket(int EventId, int UserId)
        {
            
        }

        public TicketPurchaseDto PurchaseTicket(int Id)
        {
            

        }

    }
}