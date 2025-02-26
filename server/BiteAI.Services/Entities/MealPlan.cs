namespace BiteAI.Services.Entities;

public class MealPlan
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int DailyCalories { get; set; }
    public DietType DietType { get; set; }
    public int DurationDays { get; set; } = 7;
        
    public required string UserId { get; set; }
    
    public virtual ICollection<MealDay> MealDays { get; set; } = new List<MealDay>();
}
    
public enum DietType
{
    Standard = 0,
    Vegetarian = 1,
    Vegan = 2,
    Keto = 3,
    LowCarb = 4,
    Paleo = 5,
    Mediterranean = 6,
    GlutenFree = 7,
    DairyFree = 8
}