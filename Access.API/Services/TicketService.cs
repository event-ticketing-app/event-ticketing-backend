using Access.API.DTOs;
using Access.API.Models;
using Access.API.Enums;
using Microsoft.EntityFrameworkCore;
using Access.API.Repositories;

namespace Access.API.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IPaymentService _paymentService;

        public TicketService (ITicketRepository ticketRepository, IEventRepository eventRepository, IPaymentService paymentService)
        {
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
            _paymentService = paymentService;
        }

        public  async Task<TicketReserveResponseDto> ReserveTicket(int EventId, int UserId)
        {
            var event1 = await _eventRepository.GetByIdAsync( EventId);

            if (event1 == null)
            {
                throw new Exception("Event doesn't exists");
            }

            // If the user already has a reservation for this event, send them back to it instead of failing
            var existing = await _ticketRepository.GetReservationAsync(EventId, UserId);
            if (existing != null)
            {
                if (existing.ExpiresAt > DateTime.UtcNow)
                {
                    return ToReserveDto(existing);
                }

                // A payment started just before the end may still be finishing
                if (existing.ExpiresAt.AddMinutes(TicketRules.PaymentGraceMinutes) > DateTime.UtcNow)
                {
                    throw new Exception("Your previous reservation is still being processed. Please try again in a couple of minutes.");
                }

                // Expired but the background job hasn't cancelled it yet
                existing.Status = TicketStatus.Cancelled;
                await _ticketRepository.SaveAsync();
            }

            var ticketocuppied = await _ticketRepository.CountActiveTicketsAsync(EventId);

            if (ticketocuppied >= event1.TicketCapacity)
            {
                throw new Exception("Event is full");
            }

            var newTicket = new Ticket
            {   EventId = EventId,
                UserId = UserId,
                Price = event1.Price,
                Status = TicketStatus.Reserved,
                ExpiresAt = DateTime.UtcNow.AddMinutes(TicketRules.ReservationMinutes)
            };

            await _ticketRepository.AddAsync( newTicket, event1);

            try
            {
                await _ticketRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Event is full");
            }

            return ToReserveDto(newTicket);
        }

        public async Task<string> CreatePaymentIntent(int ticketId, int userId)
        {
            var ticket = await GetOwnReservationAsync(ticketId, userId);

            if (ticket.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Ticket reservation has expired");
            }

            if (ticket.Price <= 0)
            {
                throw new Exception("Free tickets don't need a payment");
            }

            return await _paymentService.CreatePaymentIntentAsync(ticket);
        }

        public async Task<TicketPurchaseResponseDto> PurchaseTicket(int ticketId, int userId, string? paymentIntentId)
        {
            var ticket = await _ticketRepository.GetByIdWithDetailsAsync(ticketId);

            if (ticket == null || ticket.UserId != userId)
            {
                throw new Exception("Your ticket is not available");
            }

            if (ticket.Price > 0)
            {
                if (string.IsNullOrWhiteSpace(paymentIntentId) || !await _paymentService.IsPaidForTicketAsync(paymentIntentId, ticket))
                {
                    throw new Exception("Payment not completed");
                }

                // Paid, but the ticket can't be issued anymore: give the money back
                var paidInTime = ticket.ExpiresAt.AddMinutes(TicketRules.PaymentGraceMinutes) > DateTime.UtcNow;
                if (ticket.Status != TicketStatus.Reserved || !paidInTime)
                {
                    await _paymentService.RefundAsync(paymentIntentId);
                    throw new Exception("Your reservation expired before the payment finished. The payment has been refunded.");
                }
            }
            else
            {
                if (ticket.Status != TicketStatus.Reserved)
                {
                    throw new Exception("Ticket is not in reserved status");
                }

                if (ticket.ExpiresAt < DateTime.UtcNow)
                {
                    throw new Exception("Ticket reservation has expired");
                }
            }

            ticket.Status = TicketStatus.Purchased;
            ticket.PurchaseDate = DateTime.UtcNow;
            await _ticketRepository.UpdateAsync(ticket);
            await _ticketRepository.SaveAsync();

            return TicketMapper.ToPurchaseDto(ticket);
        }

        private async Task<Ticket> GetOwnReservationAsync(int ticketId, int userId)
        {
            var ticket = await _ticketRepository.GetByIdWithDetailsAsync(ticketId);

            if (ticket == null || ticket.UserId != userId)
            {
                throw new Exception("Your ticket is not available");
            }

            if (ticket.Status != TicketStatus.Reserved)
            {
                throw new Exception("Ticket is not in reserved status");
            }

            return ticket;
        }

        private static TicketReserveResponseDto ToReserveDto(Ticket ticket) => new()
        {
            Id = ticket.Id,
            EventId = ticket.EventId,
            Status = ticket.Status,
            Price = ticket.Price,
            ExpiresAt = TicketMapper.AsUtc(ticket.ExpiresAt)
        };
    }
}
