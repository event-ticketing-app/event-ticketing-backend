namespace Access.API.DTOs
{
    public class TicketPurchaseRequestDto
    {
        /// <summary>Stripe PaymentIntent id. Not needed for free tickets.</summary>
        public string? PaymentIntentId { get; set; }
    }
}
