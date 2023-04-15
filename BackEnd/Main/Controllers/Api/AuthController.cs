using Main.Contracts;
using Main.Exceptions;
using Main.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]/[action]")]
public class AuthController : Controller
{
    private readonly IAuth _auth;

    public AuthController(IAuth auth)
    {
        _auth = auth;
    }

    [HttpPost]
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

    [HttpPost]
    public async Task<IActionResult> RgisterNewUser(RegisterUserDTO registerUser)
    {
        try
        {
            await _auth.RegisterUserAsync(registerUser);
        }
        catch (AuthException e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> ChangeRoleUser(ChangeRoleUser changeRole)
    {
        try
        {
            await _auth.ChangeRoleUserAsync(changeRole);
        }
        catch (NullReferenceException e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }



}