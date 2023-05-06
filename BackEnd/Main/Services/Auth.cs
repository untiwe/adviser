using Main.Contracts.AuthController;
using Main.Exceptions;
using Main.Models;
using Main.Options;
using Main.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Main.Services;

public class Auth : IAuth
{
    private readonly DBContext _dbContext;
    private readonly IPasswordManager _passwordManager;

    public Auth(DBContext dbContext, IPasswordManager passwordManager)
    {
        _dbContext = dbContext;
        _passwordManager = passwordManager;
    }

    public string GetToken(GetTockenDTO userToken)
    {
        var username = userToken.Login;
        var password = userToken.Password;

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


    public async Task RegisterUserAsync(RegisterUserDTO registerUSer)
    {
        var login = registerUSer.Login.ToLower();

        if (_dbContext.Users.FirstOrDefault(u => u.Login == login) != null)
            throw new AuthException("Такой пользователь уже зарегистрирован");

        var password = _passwordManager.HashPassword(registerUSer.Password);
        User user = new()
        {
            Login = login,
            Password = password,
            Role = UsersRoles.User
        };
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
    

    public async Task ChangeRoleUserAsync(ChangeRoleUserDTO changeRole)
    {
        var persone = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == changeRole.User);
        if (persone == null)
        {
            throw new NullReferenceException("Пользователь с таким логином не найден");
        }
        persone.Role = changeRole.Role;
        await _dbContext.SaveChangesAsync();
    }

    public List<User> GetAllUsers()
    {
        return _dbContext.Users.ToList();
    }

    private ClaimsIdentity GetIdentity(string username, string password)
    {
        var person = _dbContext.Users.FirstOrDefault(u => u.Login == username);

        //если пользователя не найдено
        if (person == null)
            return null;

        //если не совпал пароль
        if (!_passwordManager.VerifyPassword(password, person.Password))
            return null;

        ClaimsIdentity claimsIdentity = SetClaims(person);
        return claimsIdentity;

    }

    private ClaimsIdentity SetClaims(User person)
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

}

