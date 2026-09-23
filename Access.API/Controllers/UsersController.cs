using Access.API.Models;
using Access.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Access.API.Services;
using Microsoft.AspNetCore.Authorization;
using Access.API.Repositories;


namespace Access.API.Controllers
{   
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public UsersController (IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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

            return CreatedAtAction(nameof(GetUser), new{id =responseDto.Id}, responseDto);

        }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
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

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]

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

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]

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