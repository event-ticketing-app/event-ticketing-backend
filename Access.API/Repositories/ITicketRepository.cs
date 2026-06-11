using Access.API.Models;

namespace Access.API.Repositories
{
    public interface ITicketRepository
    {
        Task <int> CountActiveTicketsAsync(int eventId);
        Task<bool> HasActiveReservationAsync(int eventId, int userId);
        Task<Ticket> AddAsync(Ticket ticket, Event eventItem);
        Task<Ticket?> GetByIdWithDetailsAsync(int id);
        Task UpdateAsync(Ticket ticket);
        Task SaveAsync();

    }
}
