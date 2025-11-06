using System.Text;
using BiteAI.Services.Contracts.Groceries;
using BiteAI.Services.Data;
using BiteAI.Services.Data.Entities;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.EntityFrameworkCore;
using BiteAI.Services.Helpers;

namespace BiteAI.Services.Services;

public class GroceryListService(AppDbContext context, ILanguageModelService languageModelService) : IGroceryListService
{
    private static readonly Dictionary<string, string> CategoryMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        { "chicken", "Meat/Protein" },
        { "beef", "Meat/Protein" },
        { "egg", "Dairy" },
        { "eggs", "Dairy" },
        { "milk", "Dairy" },
        { "tomato", "Produce" },
        { "tomatoes", "Produce" },
        { "rice", "Grains/Pasta" }
    };

    public async Task<Result<GroceryListResponseDto?>> GenerateGroceryListAsync(Guid mealPlanId, string userId, CancellationToken cancellationToken = default)
    {
        var mealPlan = await context.MealPlans
            .Include(mp => mp.MealDays)
            .ThenInclude(md => md.Meals)
            .FirstOrDefaultAsync(mp => mp.Id == mealPlanId && mp.ApplicationUserId == userId, cancellationToken);

        if (mealPlan == null)
            return Result.Fail<GroceryListResponseDto?>(OperationError.NotFound("Meal plan not found"));

        var meals = ExtractMealsFromPlan(mealPlan);
        if (meals.Count == 0)
            return Result.Fail<GroceryListResponseDto?>(OperationError.UnprocessableEntity("Meal plan has no meals"));

        var groceryListResult = await this.GenerateAndProcessGroceryListAsync(meals, mealPlanId, cancellationToken);
        if (groceryListResult.IsFailure)
            return Result.ErrorFromResult<GroceryListResponseDto?>(groceryListResult);

        var list = await context.GroceryLists.FirstOrDefaultAsync(gl => gl.MealPlanId == mealPlan.Id, cancellationToken);
        if (list == null)
        {
            list = new GroceryList
            {
                MealPlanId = mealPlan.Id,
                GeneratedAt = DateTime.UtcNow
            };
            context.GroceryLists.Add(list);
            await context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var existingItems = await context.GroceryItems.Where(i => i.GroceryListId == list.Id).ToListAsync(cancellationToken);
            if (existingItems.Count > 0)
            {
                context.GroceryItems.RemoveRange(existingItems);
                await context.SaveChangesAsync(cancellationToken);
            }

            list.GeneratedAt = DateTime.UtcNow;
        }

        foreach (var category in groceryListResult.Data!.Categories)
        {
            foreach (var item in category.Items)
            {
                context.GroceryItems.Add(new GroceryItem
                {
                    GroceryListId = list.Id,
                    CategoryName = category.Name,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    Checked = item.Checked
                });
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        var response = CreateResponse(mealPlanId, groceryListResult.Data);
        return Result.Success<GroceryListResponseDto?>(response);
    }

    private static List<MealInfo> ExtractMealsFromPlan(MealPlan mealPlan)
    {
        return mealPlan.MealDays
            .OrderBy(day => day.DayNumber)
            .SelectMany(day => day.Meals.Select(meal => new MealInfo
            {
                DayNumber = day.DayNumber,
                MealId = meal.Id,
                Name = meal.Name,
                Recipe = meal.Recipe
            }))
            .ToList();
    }

    private async Task<Result<GroceryListDto?>> GenerateAndProcessGroceryListAsync(
        List<MealInfo> meals, Guid mealPlanId, CancellationToken cancellationToken)
    {
        var userPrompt = BuildMealPrompt(meals);
        var systemPrompt = GetGroceryListSystemPrompt();

        var aiResult = await languageModelService.PromptAsync<AiGroceryListDto>(userPrompt, systemPrompt, cancellationToken);
        if (aiResult.IsFailure)
            return Result.ErrorFromResult<GroceryListDto?>(aiResult);

        var groceryList = MapToGroceryListDto(aiResult.Data, mealPlanId);
        this.ProcessAndAggregateItems(groceryList);

        return Result.Success<GroceryListDto?>(groceryList);
    }

    private static string BuildMealPrompt(IEnumerable<MealInfo> meals) =>
        string.Join(Environment.NewLine, meals.Select(m => $"Day {m.DayNumber} | {m.MealId} | {m.Name} :: {m.Recipe}"));

    private static string GetGroceryListSystemPrompt() => PromptLoader.Load("GroceryList_SystemPrompt.md");

    private static GroceryListDto MapToGroceryListDto(AiGroceryListDto? aiResponse, Guid mealPlanId)
    {
        var mealPlanIdString = mealPlanId.ToString();
        
        if (aiResponse == null)
        {
            return new GroceryListDto
            {
                WeekMealPlanId = mealPlanIdString,
                Categories = [],
                GeneratedAt = DateTime.UtcNow
            };
        }

        var categories = aiResponse.Categories?
            .Select(MapAiCategoryToGroceryCategoryDto)
            .ToList() ?? [];

        return new GroceryListDto
        {
            WeekMealPlanId = mealPlanIdString,
            Categories = categories,
            GeneratedAt = DateTime.UtcNow
        };
    }

    private static GroceryCategoryDto MapAiCategoryToGroceryCategoryDto(AiGroceryCategoryDto aiCategory)
    {
        var categoryName = !string.IsNullOrWhiteSpace(aiCategory.Name)
            ? aiCategory.Name.Trim()
            : aiCategory.CategoryName ?? "Other";

        if (string.IsNullOrWhiteSpace(categoryName))
            categoryName = "Other";

        var items = aiCategory.Items?
            .Select(MapAiItemToGroceryItemDto)
            .ToList() ?? [];

        return new GroceryCategoryDto { Name = categoryName, Items = items };
    }

    private static GroceryItemDto MapAiItemToGroceryItemDto(AiGroceryItemDto aiItem)
    {
        return new GroceryItemDto
        {
            Id = string.IsNullOrWhiteSpace(aiItem.Id) ? Guid.NewGuid().ToString() : aiItem.Id,
            Name = aiItem.Name ?? string.Empty,
            Quantity = aiItem.Quantity,
            Unit = aiItem.Unit ?? string.Empty,
            Checked = false,
            UsedInMeals = aiItem.UsedInMeals ?? []
        };
    }

    private void ProcessAndAggregateItems(GroceryListDto groceryList)
    {
        var flattenedItems = FlattenCategoryItems(groceryList.Categories);
        var aggregatedItems = this.AggregateItems(flattenedItems);
        var rebuiltCategories = GroupItemsByCategory(aggregatedItems);

        groceryList.Categories = rebuiltCategories;
        groceryList.GeneratedAt = DateTime.UtcNow;
    }

    private static List<FlattenedItem> FlattenCategoryItems(IEnumerable<GroceryCategoryDto> categories)
    {
        return categories
            .SelectMany(category => category.Items.Select(item => new FlattenedItem
            {
                CategoryName = category.Name,
                Item = item
            }))
            .ToList();
    }

    private Dictionary<string, AggregatedItem> AggregateItems(IEnumerable<FlattenedItem> flattenedItems)
    {
        var aggregated = new Dictionary<string, AggregatedItem>(StringComparer.OrdinalIgnoreCase);

        foreach (var flattened in flattenedItems)
        {
            var normalizedName = NormalizeName(flattened.Item.Name);
            var (quantity, unit) = NormalizeQuantity(flattened.Item.Quantity, flattened.Item.Unit);
            var aggregationKey = $"{normalizedName}|{unit}";

            if (!aggregated.TryGetValue(aggregationKey, out var existingItem))
            {
                var categoryName = DetermineFinalCategory(flattened.CategoryName, normalizedName);
                aggregated[aggregationKey] = new AggregatedItem
                {
                    CategoryName = categoryName,
                    Item = new GroceryItemDto
                    {
                        Id = string.IsNullOrWhiteSpace(flattened.Item.Id) ? Guid.NewGuid().ToString() : flattened.Item.Id,
                        Name = normalizedName,
                        Quantity = quantity,
                        Unit = unit,
                        Checked = false,
                        UsedInMeals = flattened.Item.UsedInMeals?.ToList() ?? []
                    }
                };
            }
            else
            {
                existingItem.Item.Quantity += quantity;
                if (flattened.Item.UsedInMeals != null)
                {
                    existingItem.Item.UsedInMeals.AddRange(flattened.Item.UsedInMeals);
                }
            }
        }

        return aggregated;
    }

    private static string DetermineFinalCategory(string originalCategory, string normalizedName)
    {
        var isUncategorized = string.IsNullOrWhiteSpace(originalCategory) ||
                              originalCategory.Equals("Uncategorized", StringComparison.OrdinalIgnoreCase);

        if (!isUncategorized)
            return originalCategory;

        foreach (var (key, value) in CategoryMappings)
        {
            if (normalizedName.Contains(key, StringComparison.OrdinalIgnoreCase))
                return value;
        }

        return "Other";
    }

    private static List<GroceryCategoryDto> GroupItemsByCategory(
        Dictionary<string, AggregatedItem> aggregatedItems)
    {
        return aggregatedItems.Values
            .GroupBy(item => item.CategoryName)
            .OrderBy(group => group.Key)
            .Select(group => new GroceryCategoryDto
            {
                Name = string.IsNullOrWhiteSpace(group.Key) ? "Other" : group.Key,
                Items = group
                    .Select(item => item.Item)
                    .OrderBy(item => item.Name)
                    .ToList()
            })
            .ToList();
    }

    private static (decimal quantity, string unit) NormalizeQuantity(decimal quantity, string? unit)
    {
        return (unit?.Trim().ToLowerInvariant() ?? "") switch
        {
            "g" or "gram" or "grams" when quantity >= 1000 => (Math.Round(quantity / 1000m, 2), "kg"),
            "g" or "gram" or "grams" => (quantity, "g"),
            "ml" or "milliliter" or "milliliters" when quantity >= 1000 => (Math.Round(quantity / 1000m, 2), "L"),
            "ml" or "milliliter" or "milliliters" => (quantity, "ml"),
            "whole" or "count" or "pcs" or "piece" or "pieces" or "" => (Math.Round(quantity, MidpointRounding.AwayFromZero), "whole"),
            _ => (quantity, (unit ?? string.Empty).Trim().ToLowerInvariant())
        };
    }

    private static string NormalizeName(string? name)
    {
        var trimmedName = (name ?? string.Empty).Trim();

        if (trimmedName.EndsWith("es", StringComparison.OrdinalIgnoreCase))
            return trimmedName[..^2];

        return trimmedName.EndsWith("s", StringComparison.OrdinalIgnoreCase) ? trimmedName[..^1] : trimmedName;
    }

    private static GroceryListResponseDto CreateResponse(Guid mealPlanId, GroceryListDto? groceryList)
    {
        return new GroceryListResponseDto
        {
            MealPlanId = mealPlanId.ToString(),
            GroceryList = groceryList!,
            GeneratedAt = DateTime.UtcNow
        };
    }
}