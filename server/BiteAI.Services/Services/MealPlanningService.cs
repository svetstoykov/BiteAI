using BiteAI.Services.Enums;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Models;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Services;

public class MealPlanningService : IMealPlanningService
{
    private readonly IAIService _aiService;

    public MealPlanningService(IAIService aiService)
    {
        this._aiService = aiService;
    }

    public async Task<Result<MealPlanDto?>> PlanMealForWeek(int days, int dailyCalorieTarget, DietTypes dietType)
    {
        var prompt = GenerateAIPrompt(days, dailyCalorieTarget, dietType);

        try
        {
            var aiResult = await this._aiService.PromptAsync<MealDayDto[]>(prompt);

            if (aiResult.IsFailure)
                return Result.ErrorFromResult<MealPlanDto?>(aiResult);

            var mealPlanDto = new MealPlanDto
            {
                CreatedDate = DateTime.UtcNow,
                DailyCalories = dailyCalorieTarget,
                DietType = dietType,
                DurationDays = days,
                MealDays = aiResult.Data!
            };

            return mealPlanDto;
        }
        catch (Exception ex)
        {
            return Result.Fail<MealPlanDto?>(OperationError.ExternalServiceError(
                "External service failed to process meal plan request",
                "AI_SERVICE_ERROR"));
        }
    }

    private static string GenerateAIPrompt(int days, int dailyCalorieTarget, DietTypes dietType)
    {
        var prompt = $$"""
                       Create a {{days}}-day meal plan with {{dailyCalorieTarget}} daily calories ({{dietType}} diet).
                       For each day, provide breakfast, lunch, and dinner with exact calories and macros (protein, carbs, fat in grams).
                       Format the response as a valid JSON object matching this C# class structure:
                        
                        public class MealDto
                       {
                           [MaxLength(30)]
                           public string Name { get; set; }
                           public string Recipe { get; set; }
                           public int Calories { get; set; }
                           public string MealType { get; set; } // Breakfast, Lunch, Dinner or Snack
                           public int ProteinInGrams { get; set; }
                           public int CarbsInGrams { get; set; }
                           public int FatInGrams { get; set; }
                       }
                       
                       public class MealDayDto
                       {
                           public int DayNumber { get; set; }
                               
                           public virtual ICollection<MealDto> Meals { get; set; } = new List<MealDto>();
                       }
                        
                       Ensure all numeric values are realistic and accurate. The JSON must be valid and match the exact structure.

                       """;
        return prompt;
    }
}