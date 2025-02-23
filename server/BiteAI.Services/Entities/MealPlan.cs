namespace BiteAI.Services.Entities;

public class MealPlan
{
    public string UserId { get; set; }
    
    public List<MealDay> Days { get; set; }
    
    public int TotalPeriodCalories { get; set; }
    
    public bool IsVegetarian { get; set; }
}