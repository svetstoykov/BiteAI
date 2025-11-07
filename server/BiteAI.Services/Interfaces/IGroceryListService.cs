using BiteAI.Services.Contracts.Groceries;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IGroceryListService
{
    Task<Result<GroceryListDto?>> GenerateGroceryListAsync(Guid mealPlanId, string userId, CancellationToken cancellationToken = default);
    Task<Result<GroceryListDto?>> GetGroceryListAsync(Guid mealPlanId, string userId, CancellationToken cancellationToken = default);
}