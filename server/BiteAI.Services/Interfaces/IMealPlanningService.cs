using BiteAI.Services.Enums;
using BiteAI.Services.Models;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IMealPlanningService
{
    Task<Result<MealPlanDto?>> GenerateMealPlanForLoggedInUserAsync(int days, int dailyCalorieTarget, DietTypes dietType, CancellationToken cancellationToken = default);
}