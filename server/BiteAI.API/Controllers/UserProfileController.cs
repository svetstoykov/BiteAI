using BiteAI.API.Controllers.Base;
using BiteAI.API.Models;
using BiteAI.Infrastructure.Data;
using BiteAI.Infrastructure.Models;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BiteAI.Services.Contracts.UserProfile;

namespace BiteAI.API.Controllers;

[Authorize]
public class UserProfileController : BaseApiController
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
        if (string.IsNullOrEmpty(userId))
            return this.ToErrorResult(Result.Fail(OperationError.Unauthorized("User ID not found.")));

        var user = await this._dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return this.ToErrorResult(Result.Fail(OperationError.NotFound("User profile not found")));

        var userProfile = new UserProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.UserName,
            Gender = user.Gender,
            Age = user.Age,
            Weight = user.Weight,
            Height = user.Height,
            ActivityLevel = user.ActivityLevel,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };

        return this.Ok(new ApiResponse<UserProfileDto>(
            success: true,
            data: userProfile
        ));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDto model)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return this.ToErrorResult(Result.Fail(OperationError.Unauthorized("User ID not found in claims")));

        var user = await this._userManager.FindByIdAsync(userId);
        if (user == null)
            return this.ToErrorResult(Result.Fail(OperationError.NotFound("User profile not found")));

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
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return this.ToErrorResult(Result.Fail(OperationError.Validation(errorMessage)));
        }

        return this.Ok(new ApiResponse<bool>(
            success: true,
            message: "User profile updated successfully",
            data: true
        ));
    }
}