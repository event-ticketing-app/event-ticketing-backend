namespace Accesso.API.Models
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}