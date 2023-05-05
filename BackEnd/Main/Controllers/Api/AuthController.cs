using FluentValidation;
using FluentValidation.Results;
using Main.Contracts;
using Main.Exceptions;
using Main.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuth _auth;
    private IValidator<ChangeRoleUserDTO> _validatorChangeRole;

    public AuthController(IAuth auth, IValidator<ChangeRoleUserDTO> validatorChangeRole)
    {
        _auth = auth;
        _validatorChangeRole = validatorChangeRole;
    }

    
    [HttpPost]
    public IActionResult Login(GetTockenDTO userToken)
    {
        try
        {
            var token = _auth.GetToken(userToken);
            return Ok(new {userToken.Login, token });
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
    public async Task<IActionResult> ChangeRoleUser(ChangeRoleUserDTO changeRole)
    {
        ValidationResult result = await _validatorChangeRole.ValidateAsync(changeRole);
        if (!result.IsValid)
            return BadRequest(result.Errors);

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