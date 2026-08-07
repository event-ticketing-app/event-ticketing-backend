using Access.API.Data;
using Access.API.Services;
using Access.API.Enums;
using Access.API.Models;
using Microsoft.EntityFrameworkCore;


namespace Access.API.Seeders
{
    public class UserSeeder
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public UserSeeder(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task SeedAsync()
        {
            var userExists = await _context.Users.AnyAsync(t => (t.Role == UserRole.Organizer) || (t.Role == UserRole.User));

            if (userExists) return;

            var password = "123";

            _passwordService.CreatePasswordHash(password, out byte[] hash1, out byte[] salt1);
            _passwordService.CreatePasswordHash(password, out byte[] hash2, out byte[] salt2);
            _passwordService.CreatePasswordHash(password, out byte[] hash3, out byte[] salt3);

            var organicerUser1 = new User
            {
                Name = "EventsCompany S.L.",
                Email = "tenant1@gmail.com",
                PasswordHash = hash1,
                PasswordSalt = salt1,
                Role = UserRole.Organizer

            };
            var organicerUser2 = new User
            {
                Name = "EventsCompany2 S.L.",
                Email = "tenant2@gmail.com",
                PasswordHash = hash2,
                PasswordSalt = salt2,
                Role = UserRole.Organizer

            };

            var user = new User
            {
                Name = "User",
                Email = "user@gmail.com",
                PasswordHash = hash3,
                PasswordSalt = salt3,
                Role = UserRole.User

            };
            _context.Users.Add(organicerUser1);
            _context.Users.Add(organicerUser2);
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

        }
    }
}
