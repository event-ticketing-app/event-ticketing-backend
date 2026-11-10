using Access.API.Enums;

namespace Access.API.DTOs
{
    public class TicketPurchaseResponseDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventName {get; set;} = string.Empty;
        public string EventDescription {get; set;} = string.Empty;
        public string UserName {get; set;} = string.Empty;
        public TicketStatus Status {get; set;}
        public decimal Price {get; set;}
        public DateTime EventDate {get; set;}
        /// <summary>Null until the ticket is purchased.</summary>
        public DateTime? PurchaseAt {get; set;}
        /// <summary>When the reservation stops holding the spot (UTC).</summary>
        public DateTime ExpiresAt {get; set;}
        public string ImageUrl { get; set; } = string.Empty;
    }


}
