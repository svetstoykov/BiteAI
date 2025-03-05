using BiteAI.Services.Entities.Base;

namespace BiteAI.Services.Entities;

public class MealDay : BaseIdentifiableEntity
{
    public int DayNumber { get; set; }
    public int TotalCalories { get; set; }
        
    public Guid MealPlanId { get; set; }
    public virtual MealPlan? MealPlan { get; set; }
        
    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}