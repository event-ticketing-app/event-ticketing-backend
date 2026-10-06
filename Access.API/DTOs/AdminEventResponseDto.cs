namespace Access.API.DTOs
{
    public class AdminEventResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int TicketCapacity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public string OrganizerEmail { get; set; } = string.Empty;
    }
}