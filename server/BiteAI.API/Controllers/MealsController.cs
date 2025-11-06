using System.Security.Claims;
using BiteAI.API.Controllers.Base;
using BiteAI.API.Models.Meals;
using BiteAI.Infrastructure.Constants;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

[Authorize]
public class MealsController(IMealPlanningService mealPlanningService, IHttpContextAccessor httpContextAccessor)
    : BaseApiController
{
    [HttpPost("weekly-meal-plan")]
    public async Task<IActionResult> GenerateWeeklyMealPlan([FromBody] GenerateMealPlanRequestDto request, CancellationToken cancellationToken = default)
    {
        var loggedInUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ExtendedClaimTypes.UniqueIdentifier);
        if (string.IsNullOrEmpty(loggedInUserId))
            return this.NoLoggedInUserResult();

        var result = await mealPlanningService.GenerateMealPlanForUserAsync(loggedInUserId, days: 7, request.DailyTargetCalories, request.DietType, cancellationToken: cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("daily-meal-plan")]
    public async Task<IActionResult> GenerateDailyMealPlan([FromBody] GenerateMealPlanRequestDto request, CancellationToken cancellationToken = default)
    {
        var loggedInUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ExtendedClaimTypes.UniqueIdentifier);
        if (string.IsNullOrEmpty(loggedInUserId))
            return this.NoLoggedInUserResult();

        var result = await mealPlanningService.GenerateMealPlanForUserAsync(loggedInUserId, days: 1, request.DailyTargetCalories, request.DietType, cancellationToken: cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("latest-meal-plan")]
    public async Task<IActionResult> GetLatestMealPlan(CancellationToken cancellationToken = default)
    {
        var loggedInUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ExtendedClaimTypes.UniqueIdentifier);
        if(string.IsNullOrEmpty(loggedInUserId))
            return this.NoLoggedInUserResult();

        var result = await mealPlanningService.GetLatestPlanForUserAsync(loggedInUserId, cancellationToken);

        return this.ToActionResult(result);
    }
}