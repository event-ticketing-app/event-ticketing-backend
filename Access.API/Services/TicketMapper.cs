using Access.API.DTOs;
using Access.API.Enums;
using Access.API.Models;

namespace Access.API.Services
{
    public static class TicketMapper
    {
        public static TicketPurchaseResponseDto ToPurchaseDto(Ticket ticket) => new()
        {
            Id = ticket.Id,
            EventId = ticket.EventId,
            EventName = ticket.Event?.Name ?? string.Empty,
            EventDescription = ticket.Event?.Description ?? string.Empty,
            UserName = ticket.User?.Name ?? string.Empty,
            Status = ticket.Status,
            Price = ticket.Price,
            EventDate = ticket.Event?.Date ?? default,
            // Only purchased tickets have a real purchase date
            PurchaseAt = ticket.Status == TicketStatus.Purchased ? AsUtc(ticket.PurchaseDate) : null,
            ExpiresAt = AsUtc(ticket.ExpiresAt),
            ImageUrl = ticket.Event?.ImageUrl ?? string.Empty
        };

        /// <summary>
        /// These dates are saved with DateTime.UtcNow, but SQL Server returns them without a time zone.
        /// Marking them as UTC makes the JSON end in "Z", so the browser converts them to local time correctly.
        /// </summary>
        public static DateTime AsUtc(DateTime date) => DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }
}
