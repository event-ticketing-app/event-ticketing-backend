using Access.API.Enums;

namespace Access.API.DTOs
{
    public class UserResponseDto
    {
        public int Id {get; set; }
        public string Name { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public  UserRole Role {get; set; }

    }
}
