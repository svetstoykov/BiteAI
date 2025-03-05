using BiteAI.Services.Data;
using BiteAI.Services.Entities;
using BiteAI.Services.Enums;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Mappings;
using BiteAI.Services.Models;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Services;

public class MealPlanningService : IMealPlanningService
{
    private readonly IAIService _aiService;
    private readonly IIdentityService _identityService;
    private readonly AppDbContext _context;
    
    public MealPlanningService(IAIService aiService, IIdentityService identityService, AppDbContext context)
    {
        this._aiService = aiService;
        this._identityService = identityService;
        this._context = context;
    }

    public async Task<Result<MealPlanDto?>> GenerateMealPlanForLoggedInUserAsync(int days, int dailyCalorieTarget, DietTypes dietType, CancellationToken cancellationToken = default)
    {
        var prompt = GetGenerateMealPlanPrompt(days, dailyCalorieTarget, dietType);
        
        var userResult = await this._identityService.GetLoggedInUserAsync(cancellationToken);
        if (userResult.IsFailure)
            return Result.ErrorFromResult<MealPlanDto?>(userResult);
        
        var aiResult = await this._aiService.PromptAsync<MealPlanDto>(prompt, cancellationToken);

        if (aiResult.IsFailure)
            return Result.ErrorFromResult<MealPlanDto?>(aiResult);

        var responseDto = aiResult.Data!;
        var currentUser = userResult.Data!;

        var mealPlan = new MealPlan
        {
            DurationDays = responseDto.MealDays.Count,
            DietType = dietType,
            CreatedDate = DateTime.UtcNow,
            DailyCalories = dailyCalorieTarget,
            ApplicationUser = currentUser,
            MealDays = responseDto.MealDays.Select(dto => dto.ToMealDayEntity()).ToList()
        };

        currentUser.MealPlans.Add(mealPlan);

        this._context.Users.Update(currentUser);
        
        await this._context.SaveChangesAsync(cancellationToken);
        
        return responseDto;
    }

    private static string GetGenerateMealPlanPrompt(int days, int dailyCalorieTarget, DietTypes dietType)
    {
        var prompt = $$"""
                       Create a {{days}}-day meal plan for a {{dietType}} diet.
                       The combined calorie count for the day *as close as possible* to the daily calorie target of {{dailyCalorieTarget}} kcal.
                       For each day, provide a single breakfast, single lunch, some snack (1-2 per day), and a single dinner with calories and macros (protein, carbs, fat in grams).
                       Provide a recipe which includes the ingredients and instructions on how to prepare the dish. Structure it in a single paragraph.
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
                               
                           public virtual ICollection<MealDto> DailyMeals { get; set; } = new List<MealDto>();
                       }
                       
                       public class MealPlanDto
                       {
                           public ICollection<MealDayDto> MealDays { get; set; } = new List<MealDayDto>();
                       }
                       
                       Ensure all numeric values are realistic and accurate. 
                       
                       The JSON Response should be a single MealPlanDto object, containing all the other nested objects inside it.
                       """;
        return prompt;
    }
}