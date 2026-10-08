using Access.API.Enums;
using Access.API.Models;

namespace Access.API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User newUser);
        Task UpdateAsync(int id, User newUser);
        Task DeleteAsync(int id);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
    }
}
