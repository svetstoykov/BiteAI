using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BiteAI.Infrastructure.Constants;
using BiteAI.Infrastructure.Settings;
using BiteAI.Services.Constants;
using BiteAI.Services.Contracts.Authentication;
using BiteAI.Services.Data;
using BiteAI.Services.Data.Entities;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BiteAI.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtConfiguration _jwtConfiguration;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        AppDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtConfiguration> jwtConfigurationOptions)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._dbContext = dbContext;
        this._httpContextAccessor = httpContextAccessor;
        this._jwtConfiguration = jwtConfigurationOptions.Value;
    }

    public async Task<Result<RegisterResponseDto>> RegisterUserAsync(RegisterDto model, CancellationToken cancellationToken = default)
    {
        var emailExists = await this._dbContext.Users
            .AnyAsync(u => u.Email == model.Email, cancellationToken: cancellationToken);
        
        if (emailExists)
            return Result.Fail<RegisterResponseDto>(OperationError.Conflict($"Username '{model.Username}' already exists"));
        
        var usernameExists = await this._dbContext.Users
            .AnyAsync(u => u.UserName == model.Username, cancellationToken: cancellationToken);
        
        if (usernameExists)
            return Result.Fail<RegisterResponseDto>(OperationError.Conflict($"Email '{model.Email}' already exists"));
        
        var applicationUser = new ApplicationUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Username,
            CreatedAt = DateTime.UtcNow
        };

        var result = await this._userManager.CreateAsync(applicationUser, model.Password);
        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Fail<RegisterResponseDto>(OperationError.Validation(errorMessage));
        }

        await this._userManager.AddToRoleAsync(applicationUser, Roles.User);
        
        return Result.Success(new RegisterResponseDto { UserId = applicationUser.Id }, "User successfully registered!");
    }

    public async Task<Result<LoginResponseDto>> LoginUserAsync(LoginDto model, CancellationToken cancellationToken = default)
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
        var expiration = DateTime.UtcNow.AddDays(this._jwtConfiguration.ExpiresInDays);

        // Create response
        var response = new LoginResponseDto
        {
            Token = token,
            FirstName = identityAccount.FirstName,
            LastName = identityAccount.LastName,
            Email = identityAccount.Email!,
            UserName = identityAccount.UserName!,
            TokenExpiration = expiration,
            UserId = Guid.Parse(identityAccount.Id)
        };

        return Result.Success(response, "Login successful!");
    }

    public async Task<Result<ApplicationUser>> GetLoggedInUserAsync(CancellationToken cancellationToken = default)
    {
        var loggedInUserId = this._httpContextAccessor.HttpContext?.User.FindFirstValue(ExtendedClaimTypes.UniqueIdentifier);
        if(string.IsNullOrEmpty(loggedInUserId))
            return Result.Fail<ApplicationUser>(OperationError.InvalidOperation("No logged in user!"));
        
        var user = await this._dbContext.Users.FirstOrDefaultAsync(u => u.Id == loggedInUserId, cancellationToken: cancellationToken);
        if (user == null)
            return Result.Fail<ApplicationUser>(OperationError.NotFound("User not found"));

        return Result.Success(user);
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser applicationUser)
    {
        var userRoles = await this._userManager.GetRolesAsync(applicationUser);

        var claims = new List<Claim>
        {
            new(ExtendedClaimTypes.UniqueIdentifier, applicationUser.Id),
            new(ClaimTypes.Name, applicationUser.UserName ?? string.Empty),
            new(ClaimTypes.Email, applicationUser.Email ?? string.Empty),
            new(ExtendedClaimTypes.FirstName, applicationUser.FirstName),
            new(ExtendedClaimTypes.LastName, applicationUser.LastName)
        };

        // Add roles to claims
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtConfiguration.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(this._jwtConfiguration.ExpiresInDays);

        var token = new JwtSecurityToken(
            issuer: this._jwtConfiguration.Issuer,
            audience: this._jwtConfiguration.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}