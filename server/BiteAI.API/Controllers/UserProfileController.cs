using BiteAI.API.Controllers.Base;
using BiteAI.API.Models;
using BiteAI.Infrastructure.Data;
using BiteAI.Infrastructure.Models;
using BiteAI.Services.Contracts.UserProfile;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BiteAI.API.Controllers;

[Authorize]
public class UserProfileController : BaseApiController
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<IdentityAccount> _userManager;

    public UserProfileController(
        AppDbContext dbContext,
        UserManager<IdentityAccount> userManager)
    {
        this._dbContext = dbContext;
        this._userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserProfile()
    {
        try
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return this.ToErrorResult(Result.Fail(OperationError.Unauthorized("User ID not found.")));

            // Get the IdentityAccount with the ApplicationUser included
            var identityAccount = await this._userManager.FindByIdAsync(userId);
            if (identityAccount == null)
                return this.ToErrorResult(Result.Fail(OperationError.NotFound("User not found")));

            // Access the ApplicationUser
            var applicationUser = identityAccount.ApplicationUser;
            if (applicationUser == null)
                return this.ToErrorResult(Result.Fail(OperationError.NotFound("User profile not found")));

            var userProfile = new UserProfileDto
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Email = applicationUser.Email,
                UserName = applicationUser.Username,
                Gender = applicationUser.Gender,
                Age = applicationUser.Age,
                Weight = applicationUser.Weight,
                Height = applicationUser.Height,
                ActivityLevel = applicationUser.ActivityLevel,
                CreatedAt = applicationUser.CreatedAt
            };

            return this.Ok(new ApiResponse<UserProfileDto>(
                success: true,
                data: userProfile
            ));
        }
        catch (Exception ex)
        {
            return this.ToErrorResult(Result.Fail(OperationError.InternalError(ex.Message)));
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDto model)
    {
        try
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return this.ToErrorResult(Result.Fail(OperationError.Unauthorized("User ID not found in claims")));

            // Get the IdentityAccount with ApplicationUser included
            var identityAccount = await this._userManager.FindByIdAsync(userId);
            if (identityAccount == null)
                return this.ToErrorResult(Result.Fail(OperationError.NotFound("User not found")));

            // Get the ApplicationUser and update it
            var applicationUser = identityAccount.ApplicationUser;
            if (applicationUser == null)
                return this.ToErrorResult(Result.Fail(OperationError.NotFound("User profile not found")));

            // Update ApplicationUser properties
            applicationUser.FirstName = model.FirstName;
            applicationUser.LastName = model.LastName;
            applicationUser.Gender = model.Gender;
            applicationUser.Age = model.Age;
            applicationUser.Weight = model.Weight;
            applicationUser.Height = model.Height;
            applicationUser.ActivityLevel = model.ActivityLevel;

            // Save changes to ApplicationUser
            this._dbContext.ApplicationUsers.Update(applicationUser);
            await this._dbContext.SaveChangesAsync();

            return this.Ok(new ApiResponse<bool>(
                success: true,
                message: "User profile updated successfully",
                data: true
            ));
        }
        catch (Exception ex)
        {
            return this.ToErrorResult(Result.Fail(OperationError.InternalError(ex.Message)));
        }
    }
}