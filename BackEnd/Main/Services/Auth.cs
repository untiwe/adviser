using Main.Exceptions;
using Main.models;
using Main.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Main.Services;

public class Auth : IAuth
{
    // тестовые данные вместо использования базы данных
    private List<Person> people = new List<Person>
        {
            new Person {Login="admin@gmail.com", Password="12345", Role = "admin" },
            new Person { Login="qwerty@gmail.com", Password="55555", Role = "user" }
        };


    public string GetToken(string username, string password)
    {
        var identity = GetIdentity(username, password);
        if (identity == null)
           throw new AuthException("Неверный логин или пароль");

        var now = DateTime.UtcNow;
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
                issuer: JWTOptions.ISSUER,
                audience: JWTOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JWTOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(JWTOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }



    private ClaimsIdentity GetIdentity(string username, string password)
    {
        Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
        if (person != null)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        // если пользователя не найдено
        return null;
    }
}

