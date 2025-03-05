namespace BiteAI.Services.Models;

public class MealDayDto
{
    public int DayNumber { get; set; }
        
    public virtual ICollection<MealDto> DailyMeals { get; set; } = new List<MealDto>();
}