using BiteAI.Services.Contracts.Enums;

namespace BiteAI.Services.Contracts.Meals;

public class MealPlanDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int DailyCalories { get; set; }
    public DietType DietType { get; set; }
    public int DurationDays { get; set; } = 7;
    public ICollection<MealDayDto> MealDays { get; set; } = new List<MealDayDto>();
}