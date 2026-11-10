using Access.API.DTOs;

namespace Access.API.Services
{
    public interface ITicketService
    {
        Task<TicketReserveResponseDto> ReserveTicket (int EventId, int UserId);
        Task<string> CreatePaymentIntent (int ticketId, int userId);
        Task<TicketPurchaseResponseDto> PurchaseTicket (int ticketId, int userId, string? paymentIntentId);
    }

}
