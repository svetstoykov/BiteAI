namespace BiteAI.Services.Entities;

public class WeeklyMealPlan
{
    public List<MealDay> Days { get; set; }
    public int TotalWeeklyCalories { get; set; }
    public bool IsVegetarian { get; set; }
}