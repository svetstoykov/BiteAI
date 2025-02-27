using BiteAI.Services.Entities.Base;

namespace BiteAI.Services.Entities;

public class MealDay : BaseIdentifiableEntity
{
    public int DayNumber { get; set; }
    public DateTime Date { get; set; }
    public int TotalCalories { get; set; }
        
    public string MealPlanId { get; set; } = null!;
    public virtual MealPlan? MealPlan { get; set; }
        
    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}