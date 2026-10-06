namespace Access.API.DTOs
{
    public class AdminEventCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int TicketCapacity { get; set; }
        public int OrganizerId { get; set; }
    }
}