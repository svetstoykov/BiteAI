using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Contracts.Meals;
using BiteAI.Services.Data;
using BiteAI.Services.Data.Entities;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Mappings;
using BiteAI.Services.Validation.Result;
using BiteAI.Services.Helpers;
using BiteAI.Services.Validation.Errors;
using Microsoft.EntityFrameworkCore;

namespace BiteAI.Services.Services;

public class MealPlanningService : IMealPlanningService
{
    private readonly ILanguageModelService _languageModelService;
    private readonly AppDbContext _context;
    
    public MealPlanningService(ILanguageModelService languageModelService, AppDbContext context)
    {
        this._languageModelService = languageModelService;
        this._context = context;
    }

    public async Task<Result<MealPlanDto?>> GenerateMealPlanForUserAsync(string userId, int days, int dailyCalorieTarget, DietType dietType, CancellationToken cancellationToken = default)
    {
        var userPrompt = GenerateUserMealPlanPrompt(days, dailyCalorieTarget, dietType);
        var systemPrompt = GetMealPlanningSystemPrompt();
        
        var aiResult = await this._languageModelService.PromptAsync<MealPlanDto>(userPrompt, systemPrompt, cancellationToken);

        if (aiResult.IsFailure) return Result.ErrorFromResult<MealPlanDto?>(aiResult);

        var responseDto = aiResult.Data;
        if (responseDto == null) return Result.Fail<MealPlanDto?>(OperationError.UnprocessableEntity("No response from AI"));

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

    private static string GenerateUserMealPlanPrompt(int days, int dailyCalorieTarget, DietType dietType)
    {
        var template = PromptLoader.Load("MealPlanning_UserPrompt.md");
        var prompt = PromptLoader.Format(template, new Dictionary<string, object>
        {
            ["days"] = days,
            ["dailyCalorieTarget"] = dailyCalorieTarget,
            ["dietType"] = dietType.ToString()
        });
        return prompt;
    }

    private static string GetMealPlanningSystemPrompt() => PromptLoader.Load("MealPlanning_SystemPrompt.md");
}