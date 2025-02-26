namespace BiteAI.Infrastructure.Models;

public class MealDay
{
    public int Id { get; set; }
    public int DayNumber { get; set; }
    public DateTime Date { get; set; }
    public int TotalCalories { get; set; }
        
    // Foreign key for meal plan
    public int MealPlanId { get; set; }
    public virtual MealPlan? MealPlan { get; set; }
        
    // Navigation properties
    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}