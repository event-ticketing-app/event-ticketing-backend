using Access.API.Enums;

namespace Access.API.DTOs
{
    public class TicketReserveResponseDto
    {
        public TicketStatus Status {get; set;}
        public decimal Price {get; set;}
        public DateTime ExpiresAt {get; set;}
    }


}

