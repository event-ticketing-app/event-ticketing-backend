namespace Access.API.Models
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
