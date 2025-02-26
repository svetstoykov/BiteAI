using BiteAI.API.Controllers.Base;
using BiteAI.Services.Contracts.Authentication;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

[AllowAnonymous]
public class AuthController : BaseApiController
{
    private readonly IIdentityService _authService;

    public AuthController(IIdentityService authService)
    {
        this._authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var result = await this._authService.RegisterUserAsync(model);

        return this.ToActionResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await this._authService.LoginUserAsync(model);
        
        return this.ToActionResult(result);
    }
}