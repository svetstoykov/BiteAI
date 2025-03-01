using BiteAI.Services.Enums;

namespace BiteAI.API.Models.Meals;

public class GenerateMealPlanRequestDto
{
    public required int DailyTargetCalories { get; set; }
    
    public required DietTypes DietType { get; set; }
}