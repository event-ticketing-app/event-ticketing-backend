using Access.API.Models;

namespace Access.API.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<Event> AddAsync(Event newEvent);
        Task UpdateAsync(int id, Event newEvent);
        Task DeleteAsync(int id);

    }
}
