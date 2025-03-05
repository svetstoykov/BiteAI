using BiteAI.Services.Enums;

namespace BiteAI.Services.Models;

public class MealPlanDto
{
    public ICollection<MealDayDto> MealDays { get; set; } = new List<MealDayDto>();
}