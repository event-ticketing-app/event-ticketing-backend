using Access.API.Data;
using Access.API.Services;
using Access.API.Enums;
using Access.API.Models;
using Microsoft.EntityFrameworkCore;


namespace Access.API.Seeders
{
    public class AdminSeeder
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public AdminSeeder(AppDbContext context, IPasswordService passwordService){
            _context = context;
            _passwordService = passwordService;
        }

        public async Task SeedAsync()
        {
            var adminExists = await _context.Users.AnyAsync(t => (t.Role == UserRole.Admin));

            if (adminExists) return;

            var email = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin@admin.com";
            var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "123";

            _passwordService.CreatePasswordHash(password, out byte[] hash, out byte[] salt);

            var adminUser = new User
            {
                Name = "Admin",
                Email= email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = UserRole.Admin

            };
            _context.Users.Add(adminUser);

            await _context.SaveChangesAsync();

        }
    }
}
