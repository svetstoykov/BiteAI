using BiteAI.Services.Entities;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IAnthropicAIService
{
    Task<Result<PeriodMealPlan?>> PlanMealForWeek(int days, int dailyCalorieTarget, bool isVegetarian);
}