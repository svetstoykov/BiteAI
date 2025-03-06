namespace BiteAI.Services.Contracts.Meals;

public class MealPlanDto
{
    public ICollection<MealDayDto> MealDays { get; set; } = new List<MealDayDto>();
}