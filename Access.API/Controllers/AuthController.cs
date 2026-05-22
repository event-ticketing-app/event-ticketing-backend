using Access.API.Data;
using Access.API.Models;
using Access.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Access.API.Services;

namespace Access.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        private readonly IPasswordService _passwordService;   

        public AuthController(AppDbContext context, ITokenService tokenService, IPasswordService passwordService)
        {
            _context=context;
            _tokenService=tokenService;
            _passwordService=passwordService;

        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            

            if (user == null)
            {
                return Unauthorized("Incorrect email.");
            }

            var verifypassword = _passwordService.VerifyPassword(dto.Password,user.PasswordHash,user.PasswordSalt);

            if (verifypassword == false)
            {
                return Unauthorized("Incorrect password.");
            }

            var token = _tokenService.CreateToken(user);

            return Ok(new { token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto dto)
        {
            _passwordService.CreatePasswordHash(dto.Password, out byte[] hash, out byte[] salt );

            var newUser = new User
            {
                Name = dto.Name,
                Email= dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = Enums.UserRole.User

            };


            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();
            var token = _tokenService.CreateToken(newUser);
            
            return Ok(new {token = token});

        }


    }
}
