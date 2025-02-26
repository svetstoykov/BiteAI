using BiteAI.API.Controllers.Base;
using BiteAI.Services.Contracts;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

[Authorize]
public class CaloriesController : BaseApiController
{
    private readonly ICalorieService _calorieService;

    public CaloriesController(ICalorieService calorieService)
    {
        this._calorieService = calorieService;
    }

    [HttpGet("calculate-goals")] 
    public IActionResult CalculateCalorieGoals(
        [FromQuery] double weightKg,
        [FromQuery] double heightCm,
        [FromQuery] int age,
        [FromQuery] int exerciseDaysPerWeek,
        [FromQuery] bool isMale)
    {
        var request = new MetabolicProfileDto()
        {
            WeightKg = weightKg,
            HeightCm = heightCm,
            Age = age,
            ExerciseDaysPerWeek = exerciseDaysPerWeek,
            IsMale = isMale
        };

        var result = this._calorieService.CalculateCalorieGoals(request);

        return this.ToActionResult(result);
    }

    [HttpGet("calculate-target-daily-calories")]
    public IActionResult CalculateTargetDailyCalories(
        [FromQuery] double weightKg,
        [FromQuery] double heightCm,
        [FromQuery] int age,
        [FromQuery] int exerciseDaysPerWeek,
        [FromQuery] bool isMale,
        [FromQuery] double targetWeightKg,
        [FromQuery] int targetWeeks)
    {
        var profile = new MetabolicProfileDto()
        {
            WeightKg = weightKg,
            HeightCm = heightCm,
            Age = age,
            ExerciseDaysPerWeek = exerciseDaysPerWeek,
            IsMale = isMale
        };
        
        var request = new WeightGoalPlanDto()
        {
            MetabolicProfile = profile,
            TargetWeightKg = targetWeightKg,
            TargetWeeks = targetWeeks
        };

        var result = this._calorieService.CalculateTargetDailyCalories(request);
        
        return this.ToActionResult(result);
    }
}