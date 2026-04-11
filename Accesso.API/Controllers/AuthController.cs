using Accesso.API.Data;
using Accesso.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Accesso.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;   

        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context=context;
            _tokenService=tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return Unauthorized("Incorrect email.");
            }

            var token = _tokenService.CreateToken(user);

            return Ok(new { token = token });
        }
    }
}