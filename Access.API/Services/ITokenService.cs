using Access.API.Models;

namespace Access.API.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }

}
