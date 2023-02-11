using Main.Exceptions;
using Main.models;
using Main.Services;
using Main.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route("api/[controller]/[action]")]
public class AuthController : Controller
{
    private readonly Auth _auth;

    public AuthController(Auth auth)
    {
        _auth = auth;
    }

    [HttpGet()]
    public string Index()
    {
        return $"Hello";
    }




    [HttpPost("/GetToken")]
    public IActionResult GetToken(string login, string password)
    {
        try
        {
            var token = _auth.GetToken(login, password);
            return Ok(token);
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }

    }
    [Authorize]
    [HttpGet("getlogin")]
    public IActionResult GetLogin()
    {
        return Ok($"Ваш логин: {User.Identity.Name}");
    }

    [Authorize(Roles = "admin")]
    [HttpGet("getrole")]
    public IActionResult GetRole()
    {
        return Ok("Ваша роль: администратор");
    }


}