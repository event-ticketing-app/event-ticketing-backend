using Access.API.Enums;

namespace Access.API.DTOs
{
    public class AdminUserCreateDto
    {

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}
