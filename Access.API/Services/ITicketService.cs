using Access.API.DTOs;

namespace Access.API.Services
{
    public interface ITicketService
    {
        Task<TicketReserveResponseDto> ReserveTicket (int EventId, int UserId);
        Task<TicketPurchaseResponseDto> PurchaseTicket (int Id);

    }

}