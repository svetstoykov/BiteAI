using BiteAI.API.Controllers.Base;
using BiteAI.Services.Contracts.Authentication;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

[AllowAnonymous]
public class AuthController(IIdentityService authService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var result = await authService.RegisterUserAsync(model);

        return this.ToActionResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await authService.LoginUserAsync(model);
        
        return this.ToActionResult(result);
    }
}