using BiteAI.Services.Contracts.Enums;

namespace BiteAI.API.Models.Meals;

public class GenerateMealPlanRequestDto
{
    public required int DailyTargetCalories { get; set; }
    
    public required DietType DietType { get; set; }
}