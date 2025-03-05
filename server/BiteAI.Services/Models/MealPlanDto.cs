using BiteAI.Services.Enums;

namespace BiteAI.Services.Models;

public class MealPlanDto
{
    public ICollection<MealDayDto> Meals { get; set; } = new List<MealDayDto>();
}