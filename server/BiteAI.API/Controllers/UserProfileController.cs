using BiteAI.Infrastructure.Data;
using BiteAI.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BiteAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserProfileController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserProfileController(
        AppDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        this._dbContext = dbContext;
        this._userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserProfile()
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
        var user = await this._dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
                
        if (user == null)
        {
            return this.NotFound();
        }

        return this.Ok(new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.UserName,
            user.Gender,
            user.Age,
            user.Weight,
            user.Height,
            user.ActivityLevel,
            user.CreatedAt,
            user.LastLoginAt
        });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDto model)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                     throw new UnauthorizedAccessException("User ID not found in claims");
            
        var user = await this._userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return this.NotFound();
        }

        // Update properties
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Gender = model.Gender;
        user.Age = model.Age;
        user.Weight = model.Weight;
        user.Height = model.Height;
        user.ActivityLevel = model.ActivityLevel;

        var result = await this._userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return this.BadRequest(result.Errors);
        }

        return this.NoContent();
    }
}

public class UserProfileUpdateDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public int? Age { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
}