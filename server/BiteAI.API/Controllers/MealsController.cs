using BiteAI.API.Controllers.Base;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

//[Authorize]
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

        return this.ToActionResult(result);
    }

    [HttpPost("daily-meal-plan")]
    public async Task<IActionResult> GenerateDailyMealPlan([FromQuery] int dailyTargetCalories, [FromQuery] bool isVegetarian) 
    {
        var result = await this._anthropicService.PlanMealForWeek(days: 1, dailyTargetCalories, isVegetarian);

        return this.ToActionResult(result);
    }
}