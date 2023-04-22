using Main.Services.Interfaces;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Main.Services
{
    public class PasswordManager : IPasswordManager
    {
        public string HashPassword(string password)
        {

            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            string saltedPassword = $"{password}{salt}";

            byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            byte[] hashBytes;
            using (var hasher = SHA256.Create())
            {
                hashBytes = hasher.ComputeHash(saltedPasswordBytes);
            }

            string hash = $"{salt}${Convert.ToBase64String(hashBytes)}";
            return hash;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split('$');
            if (parts.Length != 2)
            {
                return false;
            }
            string salt = parts[0];
            byte[] expectedHashBytes = Convert.FromBase64String(parts[1]);

            byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes($"{password}{salt}");
            byte[] actualHashBytes;
            using (var hasher = SHA256.Create())
            {
                actualHashBytes = hasher.ComputeHash(saltedPasswordBytes);
            }

            return StructuralComparisons.StructuralEqualityComparer.Equals(expectedHashBytes, actualHashBytes);
        }
    }
}
