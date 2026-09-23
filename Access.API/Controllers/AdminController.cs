using Access.API.Data;
using Access.API.Enums;
using Access.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Access.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly ITicketRepository _ticketRepository;

        private readonly IEventRepository _eventRepository;

        private readonly IUserRepository _userRepository;

        public AdminController(AppDbContext context, ITicketRepository ticketRepository, IEventRepository eventRepository, IUserRepository userRepository)
        {
            _context = context;
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult> GetDashboard()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalEvents = await _context.Events.CountAsync();
            var totalTickets = await _context.Tickets.CountAsync();
            var totalRevenue = await _context.Tickets
                .Where(t => t.Status == TicketStatus.Purchased)
                .SumAsync(t => t.Price);

            return Ok(new
            {
                totalUsers,
                totalEvents,
                totalTickets,
                totalRevenue
            });
        }
    }

}
