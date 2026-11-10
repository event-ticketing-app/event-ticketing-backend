namespace Access.API.Services
{
    public static class TicketRules
    {
        /// <summary>How long a reservation holds the spot before it has to be paid.</summary>
        public const int ReservationMinutes = 10;

        /// <summary>Extra time for a payment that started before the timer ended but finished just after.</summary>
        public const int PaymentGraceMinutes = 2;
    }
}
