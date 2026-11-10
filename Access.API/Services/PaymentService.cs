using Access.API.Models;
using Stripe;

namespace Access.API.Services
{
    public class PaymentService : IPaymentService
    {
        private const string Currency = "eur";
        private readonly StripeClient _stripeClient;

        public PaymentService(StripeClient stripeClient)
        {
            _stripeClient = stripeClient;
        }

        public async Task<string> CreatePaymentIntentAsync(Ticket ticket)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = ToCents(ticket.Price),
                Currency = Currency,
                Description = $"Ticket #{ticket.Id} - {ticket.Event?.Name}",
                // Card-like methods only, so the payment never redirects away from the app
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                    AllowRedirects = "never",
                },
                Metadata = new Dictionary<string, string>
                {
                    { "ticketId", ticket.Id.ToString() },
                    { "userId", ticket.UserId.ToString() },
                },
            };

            // Same key for the same reservation: reloading the purchase page reuses the payment instead of creating a new one.
            // ExpiresAt is included so the key stays unique if the database is reset and ticket ids repeat.
            var requestOptions = new RequestOptions { IdempotencyKey = $"ticket-{ticket.Id}-{ticket.ExpiresAt.Ticks}" };

            try
            {
                var service = new PaymentIntentService(_stripeClient);
                var paymentIntent = await service.CreateAsync(options, requestOptions);
                return paymentIntent.ClientSecret;
            }
            catch (StripeException ex)
            {
                throw new Exception($"Payment could not be started: {ex.StripeError?.Message ?? ex.Message}");
            }
        }

        public async Task<bool> IsPaidForTicketAsync(string paymentIntentId, Ticket ticket)
        {
            try
            {
                var service = new PaymentIntentService(_stripeClient);
                var paymentIntent = await service.GetAsync(paymentIntentId);

                return paymentIntent.Status == "succeeded"
                    && paymentIntent.Currency == Currency
                    && paymentIntent.AmountReceived == ToCents(ticket.Price)
                    && paymentIntent.Metadata.TryGetValue("ticketId", out var ticketId)
                    && ticketId == ticket.Id.ToString();
            }
            catch (StripeException)
            {
                return false;
            }
        }

        public async Task RefundAsync(string paymentIntentId)
        {
            var service = new RefundService(_stripeClient);
            await service.CreateAsync(new RefundCreateOptions { PaymentIntent = paymentIntentId });
        }

        private static long ToCents(decimal price) => (long)Math.Round(price * 100, MidpointRounding.AwayFromZero);
    }
}
