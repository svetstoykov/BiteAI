using System.Text;
using BiteAI.Services.Contracts.Groceries;
using BiteAI.Services.Data;
using BiteAI.Services.Data.Entities;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.EntityFrameworkCore;
using BiteAI.Services.Helpers;
using BiteAI.Services.Mappings;

namespace BiteAI.Services.Services;

public class GroceryListService(AppDbContext context, ILanguageModelService languageModelService) : IGroceryListService
{
    public async Task<Result<GroceryListDto?>> GetLatestGroceryListForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var list = await context.GroceryLists
            .Where(gl => gl.MealPlan.ApplicationUserId == userId)
            .OrderByDescending(gl =>gl.MealPlan.CreatedDate)
            .Include(gl => gl.Items)
            .FirstOrDefaultAsync(cancellationToken);

        if (list == null)
            return Result.Fail<GroceryListDto?>(OperationError.NotFound("Grocery list not found"));

        var response = list.ToGroceryListDto(list.Items);
        return Result.Success<GroceryListDto?>(response);
    }

    public async Task<Result> ToggleGroceryItemsCheckedAsync(IEnumerable<Guid> groceryListItemIds, string userId, CancellationToken cancellationToken = default)
    {
        var ids = groceryListItemIds.Distinct().ToList();
        if (ids.Count == 0)
            return Result.Fail(OperationError.Validation("No item IDs provided"));

        var items = await context.GroceryItems
            .Where(i => ids.Contains(i.Id) && i.GroceryList.MealPlan.ApplicationUserId == userId)
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
            return Result.Fail(OperationError.NotFound("No grocery items found for the provided IDs"));

        foreach (var it in items)
            it.Checked = !it.Checked;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(items.Select(i => i.ToGroceryItemDto()));
    }

    public async Task<Result<GroceryListDto?>> GetGroceryListAsync(Guid mealPlanId, string userId, CancellationToken cancellationToken = default)
    {
        var mealPlan = await context.MealPlans.FirstOrDefaultAsync(mp => mp.Id == mealPlanId && mp.ApplicationUserId == userId, cancellationToken);
        if (mealPlan == null)
            return Result.Fail<GroceryListDto?>(OperationError.NotFound("Meal plan not found"));

        var list = await context.GroceryLists
            .Include(gl => gl.Items)
            .FirstOrDefaultAsync(gl => gl.MealPlanId == mealPlan.Id, cancellationToken);

        if (list == null)
            return Result.Fail<GroceryListDto?>(OperationError.NotFound("Grocery list not found"));

        var response = list.ToGroceryListDto(list.Items);
        return Result.Success<GroceryListDto?>(response);
    }

    public async Task<Result<GroceryListDto?>> GenerateGroceryListAsync(Guid mealPlanId, string userId, CancellationToken cancellationToken = default)
    {
        var mealPlan = await context.MealPlans
            .Include(mp => mp.MealDays)
            .ThenInclude(md => md.Meals)
            .FirstOrDefaultAsync(mp => mp.Id == mealPlanId && mp.ApplicationUserId == userId, cancellationToken);

        if (mealPlan == null)
            return Result.Fail<GroceryListDto?>(OperationError.NotFound("Meal plan not found"));

        var meals = ExtractMealsFromPlan(mealPlan);
        if (meals.Count == 0)
            return Result.Fail<GroceryListDto?>(OperationError.UnprocessableEntity("Meal plan has no meals"));

        var generateGroceryList = await this.GenerateGroceryListFromMealsAsync(meals, mealPlanId, cancellationToken);
        if (generateGroceryList.IsFailure)
            return Result.ErrorFromResult<GroceryListDto?>(generateGroceryList);

        var allGroceries = generateGroceryList.Data?.ToList();
        if (allGroceries == null)
            return Result.Fail<GroceryListDto?>(OperationError.UnprocessableEntity("No valid grocery list response from AI"));

        var list = await context.GroceryLists.FirstOrDefaultAsync(gl => gl.MealPlanId == mealPlan.Id, cancellationToken);
        if (list == null)
        {
            list = mealPlan.ToNewGroceryList();
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
        
        var groceryItems = new List<GroceryItem>();
        foreach (var item in allGroceries)
        {
            var (quantity, unit) = NormalizeQuantity(item.Quantity, item.Unit);
            groceryItems.Add(item.ToGroceryItemEntity(list.Id, quantity, unit));
        }

        context.GroceryItems.AddRange(groceryItems);
        
        await context.SaveChangesAsync(cancellationToken);

        var groceryList = list.ToGroceryListDto(groceryItems);

        return Result.Success<GroceryListDto?>(groceryList);
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

    private async Task<Result<IEnumerable<GeneratedGroceryItemDto>?>> GenerateGroceryListFromMealsAsync(List<MealInfo> meals, Guid mealPlanId, CancellationToken cancellationToken)
    {
        var userPrompt = BuildMealPrompt(meals);
        var systemPrompt = GetGroceryListSystemPrompt();

        var promptResponse = await languageModelService.PromptAsync<ICollection<GeneratedGroceryItemDto>>(userPrompt, systemPrompt, cancellationToken);
        if (promptResponse.IsFailure)
            return Result.ErrorFromResult<IEnumerable<GeneratedGroceryItemDto>?>(promptResponse);

        var groceryList = promptResponse.Data;
        if (groceryList == null)
            return Result.Fail<IEnumerable<GeneratedGroceryItemDto>?>(OperationError.UnprocessableEntity("No response from AI"));
        
        return Result.Success<IEnumerable<GeneratedGroceryItemDto>?>(groceryList);
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

    private static string BuildMealPrompt(IEnumerable<MealInfo> meals) => string.Join(Environment.NewLine, meals.Select(m => $"Meal:{m.Name} | Recipe: {m.Recipe}"));

    private static string GetGroceryListSystemPrompt() => PromptLoader.Load("GroceryList_SystemPrompt.md");
}