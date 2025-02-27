using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BiteAI.Infrastructure.Data;
using BiteAI.Infrastructure.Models;
using BiteAI.Services.Constants;
using BiteAI.Services.Contracts.Authentication;
using BiteAI.Services.Entities;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BiteAI.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityAccount> _userManager;
    private readonly SignInManager<IdentityAccount> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;

    public IdentityService(
        UserManager<IdentityAccount> userManager,
        SignInManager<IdentityAccount> signInManager,
        IConfiguration configuration,
        AppDbContext dbContext)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._configuration = configuration;
        this._dbContext = dbContext;
    }

    public async Task<Result<RegisterResponseDto>> RegisterUserAsync(RegisterDto model)
    {
        var emailExists = await this._dbContext.ApplicationUsers
            .AnyAsync(u => u.Email == model.Email);
        
        if (emailExists)
            return Result.Fail<RegisterResponseDto>(OperationError.Conflict($"Username '{model.Username}' already exists"));
        
        var usernameExists = await this._dbContext.ApplicationUsers
            .AnyAsync(u => u.Username == model.Username);
        
        if (usernameExists)
            return Result.Fail<RegisterResponseDto>(OperationError.Conflict($"Email '{model.Email}' already exists"));
        
        await using var transaction = await this._dbContext.Database.BeginTransactionAsync();

        var userId = Guid.NewGuid().ToString();

        var applicationUser = new ApplicationUser
        {
            Id = userId,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Username = model.Username,
            Gender = model.Gender,
            Age = model.Age,
            WeightInKg = model.WeightInKg,
            HeightInCm = model.HeightInCm,
            ActivityLevels = model.ActivityLevel,
            CreatedAt = DateTime.UtcNow
        };

        await this._dbContext.ApplicationUsers.AddAsync(applicationUser);
        await this._dbContext.SaveChangesAsync();

        var identityAccount = new IdentityAccount
        {
            Id = userId,
            UserName = model.Username,
            Email = model.Email
        };

        var result = await this._userManager.CreateAsync(identityAccount, model.Password);
        if (!result.Succeeded)
        {
            await transaction.RollbackAsync();
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Fail<RegisterResponseDto>(OperationError.Validation(errorMessage));
        }

        await this._userManager.AddToRoleAsync(identityAccount, Roles.User);

        await transaction.CommitAsync();

        return Result.Success(new RegisterResponseDto { UserId = applicationUser.Id }, "User successfully registered");
    }

    public async Task<Result<LoginResponseDto>> LoginUserAsync(LoginDto model)
    {
        var identityAccount = await this._userManager.FindByEmailAsync(model.Email);
        if (identityAccount == null)
            return Result.Fail<LoginResponseDto>(OperationError.Unauthorized("Invalid email or password"));

        var result = await this._signInManager.CheckPasswordSignInAsync(identityAccount, model.Password, false);
        if (!result.Succeeded)
            return Result.Fail<LoginResponseDto>(OperationError.Unauthorized("Invalid email or password"));

        identityAccount.LastLoginAt = DateTime.UtcNow;

        await this._userManager.UpdateAsync(identityAccount);

        var token = await this.GenerateJwtTokenAsync(identityAccount);
        var expiration = DateTime.UtcNow.AddDays(Convert.ToDouble(this._configuration["JWT:ExpiresInDays"] ?? "7"));

        // Create response
        var response = new LoginResponseDto
        {
            Token = token,
            FirstName = identityAccount.ApplicationUser.FirstName,
            LastName = identityAccount.ApplicationUser.LastName,
            Email = identityAccount.Email!,
            UserName = identityAccount.UserName!,
            TokenExpiration = expiration,
            UserId = Guid.Parse(identityAccount.Id)
        };

        return Result.Success(response, "User successfully logged in");
    }

    private async Task<string> GenerateJwtTokenAsync(IdentityAccount identityAccount)
    {
        var userRoles = await this._userManager.GetRolesAsync(identityAccount);
        var applicationUser = identityAccount.ApplicationUser;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, identityAccount.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, identityAccount.Id),
            new(ClaimTypes.Name, identityAccount.UserName ?? string.Empty),
            new(ClaimTypes.Email, identityAccount.Email ?? string.Empty),
            new(nameof(ApplicationUser.FirstName), applicationUser.FirstName),
            new(nameof(ApplicationUser.LastName), applicationUser.LastName)
        };

        // Add roles to claims
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT:Secret not configured")));
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