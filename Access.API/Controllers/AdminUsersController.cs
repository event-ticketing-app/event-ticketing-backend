using Access.API.DTOs;
using Access.API.Enums;
using Access.API.Models;
using Access.API.Repositories;
using Access.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Access.API.Controllers
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public AdminUsersController(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        [HttpGet]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var userItem = await _userRepository.GetByIdAsync(id);
            if (userItem == null) return NotFound();

            return Ok(new UserResponseDto
            {
                Id = userItem.Id,
                Name = userItem.Name,
                Email = userItem.Email,
                Role = userItem.Role
            });
        }

        [HttpPost]
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

            return CreatedAtAction(nameof(GetUser), new { id = responseDto.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserUpdateDto dto)
        {
            var userItem = await _userRepository.GetByIdAsync(id);
            if (userItem == null) return NotFound();

            userItem.Name = dto.Name;
            userItem.Email = dto.Email;
            userItem.Role = dto.Role;

            await _userRepository.UpdateAsync(id, userItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userItem = await _userRepository.GetByIdAsync(id);
            if (userItem == null) return NotFound();

            await _userRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("organizers")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetOrganizers()
        {
            var users = await _userRepository.GetByRoleAsync(UserRole.Organizer);
            return Ok(users.Select(e => new UserResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Role = e.Role
            }));
        }

    }
}