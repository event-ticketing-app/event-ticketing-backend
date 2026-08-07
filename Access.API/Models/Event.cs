
namespace Access.API.Models
{
    public class Event
    {
        public int Id { get; set; } 
        public int OrganizerId { get; set; }
        public User? Organizer { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int TicketCapacity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public byte[] RowVersion { get; set; }
    }
}
