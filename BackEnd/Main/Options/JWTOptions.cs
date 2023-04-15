using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Main.Options
{
    public class JWTOptions
    {
        public const string ISSUER = "Adviser"; // издатель токена
        public const string AUDIENCE = "AdviserClient"; // потребитель токена
        public const int LIFETIME = 1440; // время жизни токена сутки
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTKey()));
        }

        public static string JWTKey()
        {
            string key = Environment.GetEnvironmentVariable("ASP_JWTKey");
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Environment variable no or empty");
            }
            return key;
        }
    }
}
