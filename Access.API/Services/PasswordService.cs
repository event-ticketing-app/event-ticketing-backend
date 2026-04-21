using System.Security.Cryptography;
using System.Linq;

namespace Access.API.Services
{
    public class PasswordService : IPasswordService
    { 

        public void CreatePasswordHash (string password, out byte[] hash,out byte[] salt )
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


        }

        public bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(hash);

        }
    }
}