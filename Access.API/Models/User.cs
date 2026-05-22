using Access.API.Enums;

namespace Access.API.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
        public UserRole Role { get; set; }
    }
}
