using Access.API.Enums;

namespace Access.API.DTOs
{
    public class UserCreateDto
    {
        public UserRole? Role { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;
    }
}
