using BiteAI.Services.Contracts.Groceries;
using BiteAI.Services.Data.Entities;

namespace BiteAI.Services.Mappings;

public static class GroceriesMappings
{
    // Entities -> DTOs
    public static GroceryItemDto ToGroceryItemDto(this GroceryItem entity)
        => new()
        {
            Id = entity.Id,
            Category = entity.CategoryName,
            Name = entity.Name,
            Quantity = entity.Quantity,
            Unit = entity.Unit,
            Checked = entity.Checked
        };

    public static GroceryListDto ToGroceryListDto(this GroceryList list, IEnumerable<GroceryItem> items)
        => new()
        {
            MealPlanId = list.MealPlanId.ToString(),
            GeneratedAt = list.GeneratedAt,
            Items = items.Select(i => i.ToGroceryItemDto()).ToList()
        };

    // DTOs/AI -> Entities
    public static GroceryItem ToGroceryItemEntity(this GeneratedGroceryItemDto src, Guid groceryListId, decimal quantity, string unit, bool isChecked = false)
        => new()
        {
            GroceryListId = groceryListId,
            CategoryName = src.Category,
            Name = src.Name,
            Quantity = quantity,
            Unit = unit,
            Checked = isChecked
        };

    // Helpers for creating new entities
    public static GroceryList ToNewGroceryList(this MealPlan mealPlan)
        => new()
        {
            MealPlanId = mealPlan.Id,
            GeneratedAt = DateTime.UtcNow
        };
}
