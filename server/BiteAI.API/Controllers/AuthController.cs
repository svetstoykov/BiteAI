using BiteAI.API.Models;
using BiteAI.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BiteAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
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
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

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
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
            
            return this.BadRequest(this.ModelState);
        }

        // Assign default user role
        await this._userManager.AddToRoleAsync(user, "User");

        return this.Ok(new { message = "User created successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        // Find user by email
        var user = await this._userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return this.Unauthorized(new { message = "Invalid email or password" });
        }

        // Check password
        var result = await this._signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            return this.Unauthorized(new { message = "Invalid email or password" });
        }

        // Update last login time
        user.LastLoginAt = DateTime.UtcNow;
        await this._userManager.UpdateAsync(user);

        // Generate JWT token
        var token = await this.GenerateJwtToken(user);
        var expiration = DateTime.UtcNow.AddDays(Convert.ToDouble(this._configuration["JWT:ExpiresInDays"] ?? "7"));

        return this.Ok(new LoginResponseDTO
        {
            Token = token,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty, // Email is non-nullable in our domain but Identity allows null
            UserName = user.UserName ?? string.Empty, // UserName is non-nullable in our domain but Identity allows null
            TokenExpiration = expiration,
            UserId = Guid.Parse(user.Id)
        });
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var userRoles = await this._userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName)
        };

        // Add roles to claims
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

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