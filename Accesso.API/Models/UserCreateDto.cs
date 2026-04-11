namespace Accesso.API.Models
{
    public class UserCreateDto
    {

        public string Name { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public byte[] PasswordHash { get; set; } = new byte[0];
        public string Role { get; set; } = "User";
    }
}
