using BiteAI.Services.Contracts.Common;
using BiteAI.Services.Data;
using BiteAI.Services.Validation.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using BiteAI.Services.Interfaces;

namespace BiteAI.Services.Services;

public class EnumLookupService(AppDbContext db, IMemoryCache cache) : IEnumLookupService
{
    private const string ActivityLevelsKey = "enums:activity-levels";
    private const string DietTypesKey = "enums:diet-types";
    private const string GendersKey = "enums:genders";
    private const string MealTypesKey = "enums:meal-types";
    private const string AllergiesKey = "enums:allergies";
    private const string FoodDislikesKey = "enums:food-dislikes";

    private static readonly TimeSpan DefaultTtl = TimeSpan.FromHours(24);

    private async Task<IReadOnlyList<KeyValueDto>> GetOrSetAsync(string key, Func<CancellationToken, Task<IReadOnlyList<KeyValueDto>>> factory, CancellationToken ct)
    {
        if (cache.TryGetValue(key, out IReadOnlyList<KeyValueDto>? cached) && cached is not null)
            return cached;

        var data = await factory(ct);
        cache.Set(key, data, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = DefaultTtl,
            Priority = CacheItemPriority.Normal,
            Size = data.Count
        });
        return data;
    }

    public async Task<Result<IReadOnlyList<KeyValueDto>>> GetActivityLevelsAsync(CancellationToken cancellationToken = default)
    {
        var list = await GetOrSetAsync(ActivityLevelsKey, async ct =>
            await db.ActivityLevels
                .AsNoTracking()
                .Select(x => new KeyValueDto { Key = (int)x.Value, Value = x.Name!.ToString() })
                .OrderBy(x => x.Key)
                .ToListAsync(ct), cancellationToken);
        return Result.Success<IReadOnlyList<KeyValueDto>>(list);
    }

    public async Task<Result<IReadOnlyList<KeyValueDto>>> GetDietTypesAsync(CancellationToken cancellationToken = default)
    {
        var list = await GetOrSetAsync(DietTypesKey, async ct =>
            await db.DietTypes
                .AsNoTracking()
                .Select(x => new KeyValueDto { Key = (int)x.Value, Value = x.Name!.ToString() })
                .OrderBy(x => x.Key)
                .ToListAsync(ct), cancellationToken);
        return Result.Success<IReadOnlyList<KeyValueDto>>(list);
    }

    public async Task<Result<IReadOnlyList<KeyValueDto>>> GetGendersAsync(CancellationToken cancellationToken = default)
    {
        var list = await GetOrSetAsync(GendersKey, async ct =>
            await db.Genders
                .AsNoTracking()
                .Select(x => new KeyValueDto { Key = (int)x.Value, Value = x.Name!.ToString() })
                .OrderBy(x => x.Key)
                .ToListAsync(ct), cancellationToken);
        return Result.Success<IReadOnlyList<KeyValueDto>>(list);
    }

    public async Task<Result<IReadOnlyList<KeyValueDto>>> GetMealTypesAsync(CancellationToken cancellationToken = default)
    {
        var list = await GetOrSetAsync(MealTypesKey, async ct =>
            await db.MealTypes
                .AsNoTracking()
                .Select(x => new KeyValueDto { Key = (int)x.Value, Value = x.Name!.ToString() })
                .OrderBy(x => x.Key)
                .ToListAsync(ct), cancellationToken);
        return Result.Success<IReadOnlyList<KeyValueDto>>(list);
    }

    public async Task<Result<IReadOnlyList<KeyValueDto>>> GetAllergiesAsync(CancellationToken cancellationToken = default)
    {
        var list = await GetOrSetAsync(AllergiesKey, async ct =>
            await db.AllergyTypes
                .AsNoTracking()
                .Select(x => new KeyValueDto { Key = (int)x.Value, Value = x.Name!.ToString() })
                .OrderBy(x => x.Key)
                .ToListAsync(ct), cancellationToken);
        return Result.Success<IReadOnlyList<KeyValueDto>>(list);
    }

    public async Task<Result<IReadOnlyList<KeyValueDto>>> GetFoodDislikesAsync(CancellationToken cancellationToken = default)
    {
        var list = await GetOrSetAsync(FoodDislikesKey, async ct =>
            await db.FoodDislikeTypes
                .AsNoTracking()
                .Select(x => new KeyValueDto { Key = (int)x.Value, Value = x.Name!.ToString() })
                .OrderBy(x => x.Key)
                .ToListAsync(ct), cancellationToken);
        return Result.Success<IReadOnlyList<KeyValueDto>>(list);
    }
}