using Access.API.Data;
using Access.API.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Access.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
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