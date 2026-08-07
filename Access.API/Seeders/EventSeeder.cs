using Access.API.Data;
using Access.API.Enums;
using Access.API.Models;
using Access.API.Services;
using Microsoft.EntityFrameworkCore;


namespace Access.API.Seeders
{
    public class EventSeeder
    {
        private readonly AppDbContext _context;

        public EventSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var eventExists = await _context.Events.AnyAsync();

            if (eventExists) return;

            var organizer1 = await _context.Users.FirstOrDefaultAsync(U => U.Email == "tenant1@gmail.com");
            var organizer2 = await _context.Users.FirstOrDefaultAsync(U => U.Email == "tenant2@gmail.com");

            var event1 = new Event
            {
                OrganizerId= organizer1!.Id,
                Name = "Event1",
                Description="This is an event",
                Date = new DateTime(2026, 12, 15),
                Price = 7.55m,
                TicketCapacity = 10

            };
            var event2 = new Event
            {
                OrganizerId = organizer2!.Id,
                Name = "Event2",
                Description = "This is an event2",
                Date = new DateTime(2026, 12, 15),
                Price = 10.55m,
                TicketCapacity = 20

            };


            _context.Events.Add(event1);
            _context.Events.Add(event2);

            await _context.SaveChangesAsync();

        }
    }
}
