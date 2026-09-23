using Access.API.Data;
using Access.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Access.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> AddAsync(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task UpdateAsync(int id, User newUser)
        {
            var userItem = await _context.Users.FindAsync(id);

            if (userItem == null)
            {
                return;
            }

            userItem.Name = newUser.Name;
            userItem.Email = newUser.Email;
            userItem.Role = newUser.Role;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var userItem = await _context.Users.FindAsync(id);

            if (userItem == null)
            {
                return;
            }

            _context.Users.Remove(userItem);
            await _context.SaveChangesAsync();
        }
    }
}
