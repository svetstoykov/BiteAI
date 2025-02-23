using BiteAI.Services.Entities;

namespace BiteAI.Services.Interfaces;

public interface IAnthropicAIService
{
    Task<WeeklyMealPlan> PlanMealForWeek(int targetWeeklyCalories, bool isVegetarian);
}