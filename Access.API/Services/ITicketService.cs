using Access.API.DTOs;

namespace Access.API.Services
{
    public interface ITicketService
    {
        TicketReserveResponseDto ReserveTicket (int EventId, int UserId);
        TicketPurchaseResponseDto PurchaseTicket (int Id);

    }

}