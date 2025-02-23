namespace BiteAI.Services.Entities;

public class PeriodMealPlan
{
    public List<MealDay> Days { get; set; }
    public int TotalPeriodCalories { get; set; }
    public bool IsVegetarian { get; set; }
}