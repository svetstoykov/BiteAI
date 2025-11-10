using BiteAI.Services.Contracts.Common;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IEnumLookupService
{
    Task<Result<IReadOnlyList<KeyValueDto>>> GetActivityLevelsAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<KeyValueDto>>> GetDietTypesAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<KeyValueDto>>> GetGendersAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<KeyValueDto>>> GetMealTypesAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<KeyValueDto>>> GetAllergiesAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<KeyValueDto>>> GetFoodDislikesAsync(CancellationToken cancellationToken = default);
}