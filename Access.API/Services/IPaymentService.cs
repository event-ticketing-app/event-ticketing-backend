using Access.API.Models;

namespace Access.API.Services
{
    public interface IPaymentService
    {
        /// <summary>Creates (or returns the existing) Stripe PaymentIntent for a reserved ticket and returns its client secret.</summary>
        Task<string> CreatePaymentIntentAsync(Ticket ticket);

        /// <summary>True when the PaymentIntent was paid in full and belongs to this ticket.</summary>
        Task<bool> IsPaidForTicketAsync(string paymentIntentId, Ticket ticket);

        /// <summary>Gives the money back when a payment can't be turned into a ticket.</summary>
        Task RefundAsync(string paymentIntentId);
    }
}
