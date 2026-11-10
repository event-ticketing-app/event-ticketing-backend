using Access.API.Enums;

namespace Access.API.DTOs
{
    public class EventResponseDto
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int TicketCapacity {get; set;}
        public EventStatus Status { get; set; }
        public int OrganizerId { get; set; }
        public string Location { get; set; } = string.Empty;
        /// <summary>Reserved + purchased tickets. Spots left = TicketCapacity - TicketsSold.</summary>
        public int TicketsSold { get; set; }
    }
}
