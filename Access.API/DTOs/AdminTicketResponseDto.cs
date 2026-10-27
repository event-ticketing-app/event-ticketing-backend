using Access.API.Enums;

namespace Access.API.DTOs
{
    public class AdminTicketResponseDto
    {
        public int Id { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventDescription { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
        public decimal Price { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
