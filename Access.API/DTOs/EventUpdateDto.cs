using System.ComponentModel.DataAnnotations;

namespace Access.API.DTOs
{
    public class EventUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        [Range(0, double.MaxValue)]
                public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
                public int TicketCapacity { get; set; }
    }
}