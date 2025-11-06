using BiteAI.Services.Data.Entities.Base;

namespace BiteAI.Services.Data.Entities;

public class GroceryList : BaseIdentifiableEntity
{
    public Guid MealPlanId { get; set; }
    public MealPlan MealPlan { get; set; } = null!;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<GroceryItem> Items { get; set; } = new List<GroceryItem>();
}
