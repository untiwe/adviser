using Main.Exceptions;
using Main.models;
using Main.Services;
using Main.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route("api/[controller]/[action]")]
public class AuthController : Controller
{
    private readonly IAuth _auth;

    public AuthController(IAuth auth)
    {
        _auth = auth;
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
    [HttpGet]
    public IActionResult testc()
    {
        return NoContent();
    }
}