using BiteAI.API.Models;
using BiteAI.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BiteAI.API.Controllers.Base;
using BiteAI.API.Models.DTOs.Authentication;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.AspNetCore.Authorization;

namespace BiteAI.API.Controllers;

[AllowAnonymous]
public class AuthController : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Gender = model.Gender,
            Age = model.Age,
            Weight = model.Weight,
            Height = model.Height,
            ActivityLevel = model.ActivityLevel,
            CreatedAt = DateTime.UtcNow
        };

        var result = await this._userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            var error = OperationError.Validation(errorMessage);
            return this.ToErrorResult(Result.Fail(error));
        }

        // Assign default user role
        await this._userManager.AddToRoleAsync(user, "User");

        return this.Ok(new ApiResponse<RegisterResponseDto>(
            success: true,
            message: "User registered successfully",
            data: new RegisterResponseDto {UserId = user.Id}
        ));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await this._userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            var error = OperationError.Unauthorized("Invalid email or password");
            return this.ToErrorResult(Result.Fail(error));
        }

        var result = await this._signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            var error = OperationError.Unauthorized("Invalid email or password");
            return this.ToErrorResult(Result.Fail(error));
        }

        user.LastLoginAt = DateTime.UtcNow;
        await this._userManager.UpdateAsync(user);

        var token = await this.GenerateJwtToken(user);
        var expiration = DateTime.UtcNow.AddDays(Convert.ToDouble(this._configuration["JWT:ExpiresInDays"] ?? "7"));

        var response = new LoginResponseDto
        {
            Token = token,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            UserName = user.UserName!,
            TokenExpiration = expiration,
            UserId = Guid.Parse(user.Id)
        };

        return this.Ok(new ApiResponse<LoginResponseDto>(
            success: true,
            message: "Login successful",
            data: response
        ));
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var userRoles = await this._userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["JWT:Secret"] ?? 
                                                                  throw new InvalidOperationException("JWT:Secret not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(this._configuration["JWT:ExpiresInDays"] ?? "7"));

        var token = new JwtSecurityToken(
            issuer: this._configuration["JWT:Issuer"],
            audience: this._configuration["JWT:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}