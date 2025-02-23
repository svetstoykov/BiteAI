using BiteAI.API.Controllers.Base;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

public class MealsController : BaseApiController
{
    private readonly IAnthropicAIService _anthropicService;

    public MealsController(IAnthropicAIService anthropicService)
    {
        this._anthropicService = anthropicService;
    }

    [HttpPost("weekly-meal-plan")]
    public async Task<IActionResult> GenerateWeeklyMealPlan([FromQuery] int dailyTargetCalories, [FromQuery] bool isVegetarian)
    {
        var result = await this._anthropicService.PlanMealForWeek(days: 7, dailyTargetCalories, isVegetarian);

        return result.IsFailure ? this.ToErrorResult(result) : this.Ok(result.Data);
    }

    [HttpPost("daily-meal-plan")]
    public async Task<IActionResult> GenerateDailyMealPlan([FromQuery] int dailyTargetCalories, [FromQuery] bool isVegetarian) 
    {
        var result = await this._anthropicService.PlanMealForWeek(days: 1, dailyTargetCalories, isVegetarian);

        return result.IsFailure ? this.ToErrorResult(result) : this.Ok(result.Data);
    }
}