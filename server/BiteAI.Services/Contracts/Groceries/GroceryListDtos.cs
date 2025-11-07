namespace BiteAI.Services.Contracts.Groceries;

public class GroceryListDto
{
    public string MealPlanId { get; set; } = string.Empty;
    public List<GroceryItemDto> Items { get; set; } = new();
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}

public class GroceryItemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public bool Checked { get; set; } = false;
}

public class GeneratedGroceryItemDto
{
    public required string Name { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
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