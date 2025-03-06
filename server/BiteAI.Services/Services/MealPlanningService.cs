using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Contracts.Meals;
using BiteAI.Services.Data;
using BiteAI.Services.Data.Entities;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Mappings;
using BiteAI.Services.Validation.Result;
using Microsoft.EntityFrameworkCore;

namespace BiteAI.Services.Services;

public class MealPlanningService : IMealPlanningService
{
    private readonly IAIService _aiService;
    private readonly AppDbContext _context;
    
    public MealPlanningService(IAIService aiService, AppDbContext context)
    {
        this._aiService = aiService;
        this._context = context;
    }

    public async Task<Result<MealPlanDto?>> GenerateMealPlanForUserAsync(string userId, int days, int dailyCalorieTarget, DietTypes dietType, CancellationToken cancellationToken = default)
    {
        var prompt = GetGenerateMealPlanPrompt(days, dailyCalorieTarget, dietType);
        
        var aiResult = await this._aiService.PromptAsync<MealPlanDto>(prompt, cancellationToken);

        if (aiResult.IsFailure)
            return Result.ErrorFromResult<MealPlanDto?>(aiResult);

        var responseDto = aiResult.Data!;

        var mealPlan = new MealPlan
        {
            DurationDays = responseDto.MealDays.Count,
            DietType = dietType,
            CreatedDate = DateTime.UtcNow,
            DailyCalories = dailyCalorieTarget,
            ApplicationUserId = userId,
            MealDays = responseDto.MealDays.Select(dto => dto.ToMealDayEntity()).ToList()
        };


        this._context.MealPlans.Add(mealPlan);
        
        await this._context.SaveChangesAsync(cancellationToken);
        
        return responseDto;
    }

    public async Task<Result<MealPlanDto?>> GetLatestPlanForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var mealPlan = await this._context.MealPlans
            .OrderByDescending(mealPlan => mealPlan.CreatedDate)
            .Include(m => m.MealDays)
                .ThenInclude(md => md.Meals)
            .FirstOrDefaultAsync(m => m.ApplicationUserId == userId, cancellationToken);

        if (mealPlan == null)
            return Result.Success(default(MealPlanDto?));
        
        var mealPlanDto = mealPlan.ToMealPlanDto();
        
        return Result.Success(mealPlanDto)!;
    }

    private static string GetGenerateMealPlanPrompt(int days, int dailyCalorieTarget, DietTypes dietType)
    {
        var prompt = $$"""
                       Create a {{days}}-day meal plan for a {{dietType}} diet.
                       For each day, provide a single breakfast, single lunch, some snack (1-2 per day), and a single dinner with calories and macros (protein, carbs, fat in grams).
                       The combined calorie count for the day from all meals should be *as close as possible* to the daily calorie target of {{dailyCalorieTarget}} kcal.
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