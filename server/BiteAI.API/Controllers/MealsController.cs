using BiteAI.API.Controllers.Base;
using BiteAI.API.Models.Meals;
using BiteAI.Services.Enums;
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
    public async Task<IActionResult> GenerateWeeklyMealPlan([FromBody] GenerateMealPlanRequestDto request)
    {
        var result = await this._anthropicService.PlanMealForWeek(days: 7, request.DailyTargetCalories, request.DietType);

        return this.ToActionResult(result);
    }

    [HttpPost("daily-meal-plan")]
    public async Task<IActionResult> GenerateDailyMealPlan([FromBody] GenerateMealPlanRequestDto request)
    {
        var result = await this._anthropicService.PlanMealForWeek(days: 1, request.DailyTargetCalories, request.DietType);

        return this.ToActionResult(result);
    }
}