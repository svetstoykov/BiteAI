using BiteAI.Services.Enums;

namespace BiteAI.Services.Models;

public class MealPlanDto
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int DailyCalories { get; set; }
    public DietTypes DietType { get; set; }
    public int? DurationDays { get; set; }
    public ICollection<MealDayDto> MealDays { get; set; } = new List<MealDayDto>();
}