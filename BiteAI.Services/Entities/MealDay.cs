namespace BiteAI.Services.Entities;

public class MealDay
{
    public string DayOfWeek { get; set; }
    public Meal Breakfast { get; set; }
    public Meal Lunch { get; set; }
    public Meal Dinner { get; set; }
    public int TotalCalories { get; set; }
}