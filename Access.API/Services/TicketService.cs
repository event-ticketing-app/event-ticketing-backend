using Access.API.Data;
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

        public TicketService (ITicketRepository ticketRepository, IEventRepository eventRepository)
        {
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;

        }
        public  async Task<TicketReserveResponseDto> ReserveTicket(int EventId, int UserId)
        {

            var event1 = await _eventRepository.GetByIdAsync( EventId);

            if (event1 == null)
            {
                throw new Exception("Event doesn't exists");
            }
            var ticketocuppied = await _ticketRepository.CountActiveTicketsAsync(EventId);

            if (ticketocuppied >= event1.TicketCapacity)
            {
                throw new Exception("Event is full");

            }

            var ticketuser = await _ticketRepository.HasActiveReservationAsync(EventId,UserId);
            
            if (ticketuser)
            {
                throw new Exception("You have an active reserve");
            }
            

            var newTicket = new Ticket
            {   EventId = EventId,
                UserId = UserId,
                Price = event1.Price,
                Status = TicketStatus.Reserved,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)

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

            var responseDTO = new TicketReserveResponseDto
            {
                Id = newTicket.Id,
                Status =  newTicket.Status,
                Price =  newTicket.Price,
                ExpiresAt = newTicket.ExpiresAt

            };

            return responseDTO;

        }

        public async Task<TicketPurchaseResponseDto> PurchaseTicket(int Id)
        {
            var ticket = await _ticketRepository.GetByIdWithDetailsAsync(Id);

            if (ticket == null)
            {
                throw new Exception("Your ticket is not available");
            }

            if (ticket.Status != TicketStatus.Reserved)
            {
                throw new Exception("Ticket is not in reserved status");
            }

            if (ticket.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Ticket reservation has expired");
            }

            ticket.Status = TicketStatus.Purchased;
            await _ticketRepository.UpdateAsync(ticket);
            await _ticketRepository.SaveAsync();

            var responseDTO =  new TicketPurchaseResponseDto
            {
                EventName = ticket.Event.Name,
                EventDescription = ticket.Event.Description,
                UserName = ticket.User.Name,
                Status = ticket.Status,
                Price = ticket.Price,
                EventDate = ticket.Event.Date,
                PurchaseAt = DateTime.UtcNow


            };

            return responseDTO;
        }

    }
}