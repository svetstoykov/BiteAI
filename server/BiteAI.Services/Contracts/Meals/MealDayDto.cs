namespace BiteAI.Services.Contracts.Meals;

public class MealDayDto
{
    public int DayNumber { get; set; }
        
    public int TotalCalories { get; set; }
    
    public virtual ICollection<MealDto> DailyMeals { get; set; } = new List<MealDto>();
}