using BiteAI.Services.Data;
using BiteAI.Services.Entities;
using BiteAI.Services.Enums;
using BiteAI.Services.Interfaces;
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
        var prompt = GenerateAIPrompt(days, dailyCalorieTarget, dietType);

        var aiResult = await this._aiService.PromptAsync<MealDayDto[]>(prompt, cancellationToken);

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

        var userResult = await this._identityService.GetLoggedInUserAsync(cancellationToken);
        if (userResult.IsFailure)
            return Result.ErrorFromResult<MealPlanDto?>(userResult);
        
        var currentUser = userResult.Data!;
        
        var mealPlanEntity = MapToDomainEntity(mealPlanDto);

        currentUser.MealPlans.Add(mealPlanEntity);

        this._context.Users.Update(currentUser);
        
        await this._context.SaveChangesAsync(cancellationToken);
        
        return mealPlanDto;
    }

    private static MealPlan MapToDomainEntity(MealPlanDto mealPlanDto)
    {
        var mealPlan = new MealPlan()
        {
            DurationDays = mealPlanDto.MealDays.Count,
            DietType = mealPlanDto.DietType,
            CreatedDate = mealPlanDto.CreatedDate,
            DailyCalories = mealPlanDto.DailyCalories,
        };

        var mealDays = mealPlanDto.MealDays.Select(mealDayDto => new MealDay()
            {
                DayNumber = mealDayDto.DayNumber,
                Meals = mealDayDto.Meals.Select(m => new Meal()
                    {
                        Name = m.Name,
                        Recipe = m.Recipe,
                        CarbsInGrams = m.CarbsInGrams,
                        FatInGrams = m.FatInGrams,
                        ProteinInGrams = m.ProteinInGrams,
                        Calories = m.Calories,
                        MealType = Enum.Parse<MealTypes>(m.MealType)
                    })
                    .ToList()
            })
            .ToList();

        mealPlan.MealDays = mealDays;

        return mealPlan;
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