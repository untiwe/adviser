using Main.Contracts;
using Main.Exceptions;
using Main.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuth _auth;

    public AuthController(IAuth auth)
    {
        _auth = auth;
    }

    
    [HttpPost]
    public IActionResult GetToken(GetTockenDTO userToken)
    {
        try
        {
            var token = _auth.GetToken(userToken);
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


    [HttpGet]
    public IActionResult GelAllUsers()
    {
            return Ok(_auth.GetAllUsers());
      
    }


}