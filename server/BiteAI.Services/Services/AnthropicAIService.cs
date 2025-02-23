using System.Text.Json;
using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using BiteAI.Services.Entities;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Services;

public class AnthropicAIService : IAnthropicAIService
{
    private readonly AnthropicClient _anthropicClient;

    public AnthropicAIService(AnthropicClient anthropicClient)
    {
        this._anthropicClient = anthropicClient;
    }

    public async Task<Result<MealPlan?>> PlanMealForWeek(int days, int dailyCalorieTarget, bool isVegetarian)
    {
        var dietType = isVegetarian ? "vegetarian" : "regular";

        var prompt = GenerateAIPrompt(days, dailyCalorieTarget, dietType);

        try
        {
            var messages = new List<Message>()
            {
                new(RoleType.User, prompt),
            };

            var parameters = new MessageParameters()
            {
                Messages = messages,
                MaxTokens = 4096,
                Model = AnthropicModels.Claude35Sonnet,
                Stream = false,
                Temperature = 0.7m,
            };

            var firstResult = await this._anthropicClient.Messages.GetClaudeMessageAsync(parameters);

            var contentResult = firstResult.Message?.ToString();

            if (contentResult == null)
                return Result.Fail<MealPlan?>(OperationError.ExternalServiceError(
                    "Failed to generate response from AI service",
                    "AI_NO_RESPONSE"));

            MealPlan? periodMealPlan;
            try
            {
                periodMealPlan = JsonSerializer.Deserialize<MealPlan>(contentResult);
            }
            catch (Exception e)
            {
                return Result.Fail<MealPlan?>(OperationError.InternalError(
                    "Failed to parse response from AI service", "RESPONSE_PARSE_ERROR"));
            }

            return Result.Success(periodMealPlan);
        }
        catch (Exception ex)
        {
            return Result.Fail<MealPlan?>(OperationError.ExternalServiceError(
                "External service failed to process meal plan request",
                "AI_SERVICE_ERROR"));
        }
    }

    private static string GenerateAIPrompt(int days, int dailyCalorieTarget, string dietType)
    {
        var prompt = $$"""
                       Create a {{days}}-day meal plan with {{dailyCalorieTarget}} daily calories ({{dietType}} diet).
                       For each day, provide breakfast, lunch, and dinner with exact calories and macros (protein, carbs, fat in grams).
                       Format the response as a valid JSON object matching this C# class structure:

                       public class PeriodMealPlan {
                           public List<MealDay> Days { get; set; }
                           public int TotalPeriodCalories { get; set; }
                           public bool IsVegetarian { get; set; }
                       }

                       public class MealDay {
                           public string DayOfWeek { get; set; }
                           public Meal Breakfast { get; set; }
                           public Meal Lunch { get; set; }
                           public Meal Dinner { get; set; }
                           public int TotalCalories { get; set; }
                       }

                       public class Meal {
                           public string Name { get; set; }
                           public List<string> Ingredients { get; set; }
                           public string PreparationSteps { get; set; }
                           public int Calories { get; set; }
                           public Dictionary<string, double> Macros { get; set; }
                       }

                       Ensure all numeric values are realistic and accurate. The JSON must be valid and match the exact structure.
                       """;
        return prompt;
    }
}