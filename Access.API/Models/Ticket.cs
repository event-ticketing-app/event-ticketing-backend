using Access.API.Enums;


namespace Access.API.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int EventId {get; set;}
        public int UserId {get; set;}
        public Event Event {get; set;}
        public User User {get; set;}
        public decimal Price {get; set;}
        public DateTime PurchaseDate {get; set;}
        public TicketStatus Status {get; set;}
        public DateTime ExpiresAt {get; set;}
    }

}