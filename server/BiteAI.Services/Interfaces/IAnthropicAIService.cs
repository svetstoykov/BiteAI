using BiteAI.Services.Entities;
using BiteAI.Services.Enums;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IAnthropicAIService
{
    Task<Result<MealPlan?>> PlanMealForWeek(int days, int dailyCalorieTarget, DietTypes dietType);
}