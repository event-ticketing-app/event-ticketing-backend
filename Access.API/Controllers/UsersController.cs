using Access.API.Data;
using Access.API.Models;
using Access.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Access.API.Services;
using Microsoft.AspNetCore.Authorization;


namespace Access.API.Controllers
{   
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public UsersController (AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService=passwordService;

        }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users.Select(e => new UserResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Role = e.Role
            }));
        }
    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> PostUser(UserCreateDto dto)
        {
            _passwordService.CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt);
            


            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var responseDto = new UserResponseDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
                Role = newUser.Role
            };
            
            return CreatedAtAction(nameof(GetUser), new{id =responseDto.Id}, responseDto);

        }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
        {
            var userItem = await _context.Users.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }
            _context.Users.Remove(userItem);
            
            await _context.SaveChangesAsync();


            return NoContent();

        }


    [HttpPut("{id}")]

    public async Task<IActionResult> PutUser(int id, UserUpdateDto dto)
        {
            var userItem = await _context.Users.FindAsync(id);
            if (userItem == null)
            {
                return NotFound();
            }
            userItem.Name = dto.Name;
            userItem.Email = dto.Email;
            userItem.Role = dto.Role;

            await _context.SaveChangesAsync();

            return NoContent();
        }


    [HttpGet("{id}")]
    
    public async Task<ActionResult<UserResponseDto>> GetUser (int id)
        {

            var userItem = await _context.Users.FindAsync(id);

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