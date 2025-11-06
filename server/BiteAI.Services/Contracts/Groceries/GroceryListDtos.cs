namespace BiteAI.Services.Contracts.Groceries;

public class GroceryListDto
{
    public required string WeekMenuId { get; set; }
    public List<GroceryCategoryDto> Categories { get; set; } = new();
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}

public class GroceryCategoryDto
{
    public required string Name { get; set; }
    public List<GroceryItemDto> Items { get; set; } = new();
}

public class GroceryItemDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = "";
    public bool Checked { get; set; }
    public List<string> UsedInMeals { get; set; } = new();
}

public class GroceryListResponseDto
{
    public required string MenuId { get; set; }
    public required GroceryListDto GroceryList { get; set; }
    public DateTime GeneratedAt { get; set; }
}

// AI parsing DTOs without required members for LLM deserialization
public class AiGroceryListDto
{
    public string? WeekMenuId { get; set; }
    public List<AiGroceryCategoryDto>? Categories { get; set; }
    public DateTime GeneratedAt { get; set; }
}

public class AiGroceryCategoryDto
{
    public string? CategoryName { get; set; }
    public string? Name { get; set; }
    public List<AiGroceryItemDto>? Items { get; set; }
}

public class AiGroceryItemDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public bool Checked { get; set; }
    public List<string>? UsedInMeals { get; set; }
}

public record MealInfo
{
    public int DayNumber { get; init; }
    public Guid MealId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Recipe { get; init; } = string.Empty;
}

public class FlattenedItem
{
    public string CategoryName { get; init; } = string.Empty;
    public GroceryItemDto Item { get; init; } = null!;
}

public class AggregatedItem
{
    public string CategoryName { get; init; } = string.Empty;
    public GroceryItemDto Item { get; init; } = null!;
}