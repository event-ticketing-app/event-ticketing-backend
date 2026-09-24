using Access.API.Data;
using Access.API.DTOs;
using Access.API.Enums;
using Access.API.Models;
using Access.API.Repositories;
using Access.API.Services;
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

        private readonly IPasswordService _passwordService;

        public AdminController(AppDbContext context, ITicketRepository ticketRepository, IEventRepository eventRepository, IUserRepository userRepository, IPasswordService passwordService)
        {
            _context = context;
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _passwordService = passwordService;
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

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return Ok(users.Select(e => new UserResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Role = e.Role
            }));
        }

        [HttpPost("users")]
        public async Task<ActionResult<UserResponseDto>> PostUser(AdminUserCreateDto dto)
        {
            _passwordService.CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);

            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = dto.Role
            };

            await _userRepository.AddAsync(newUser);

            var responseDto = new UserResponseDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
                Role = newUser.Role
            };

            return CreatedAtAction(nameof(GetUser), new{id =responseDto.Id}, responseDto);

        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userItem = await _userRepository.GetByIdAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteAsync(id);

            return NoContent();

        }

        [HttpPut("users/{id}")]

        public async Task<IActionResult> PutUser(int id, UserUpdateDto dto)
        {
            var userItem = await _userRepository.GetByIdAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }

            userItem.Name = dto.Name;
            userItem.Email = dto.Email;
            userItem.Role = dto.Role;

            await _userRepository.UpdateAsync(id, userItem);

            return NoContent();
        }

        [HttpGet("users/{id}")]

        public async Task<ActionResult<UserResponseDto>> GetUser (int id)
        {

            var userItem = await _userRepository.GetByIdAsync(id);

            if (userItem == null)
            {
                return NotFound();

            }

            var responseDto = new UserResponseDto
            {
                Id = userItem.Id,
                Name = userItem.Name,
                Email = userItem.Email,
                Role = userItem.Role
            };
            return responseDto;

        }
    }

}
