using Access.API.Enums;

namespace Access.API.DTOs
{
    public class UserUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public  UserRole Role {get; set; }

    }
}
