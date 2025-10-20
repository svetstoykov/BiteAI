using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Contracts.Meals;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IMealPlanningService
{
    Task<Result<MealPlanDto?>> GenerateMealPlanForUserAsync(string userId, int days, int dailyCalorieTarget, DietType dietType, CancellationToken cancellationToken = default);
    
    Task<Result<MealPlanDto?>> GetLatestPlanForUserAsync(string userId, CancellationToken cancellationToken = default);
}