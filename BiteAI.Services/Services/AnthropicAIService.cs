using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using BiteAI.Services.Entities;
using BiteAI.Services.Interfaces;

namespace BiteAI.Services.Services;

public class AnthropicAIService : IAnthropicAIService
{
    private readonly AnthropicClient _anthropicClient;

    public AnthropicAIService(AnthropicClient anthropicClient)
    {
        _anthropicClient = anthropicClient;
    }

    public async Task<WeeklyMealPlan> PlanMealForWeek(int targetWeeklyCalories, bool isVegetarian)
    {
        var dailyCalorieTarget = targetWeeklyCalories / 7;
        var dietType = isVegetarian ? "vegetarian" : "regular";

        var prompt = $$"""
                       Create a 7-day meal plan with {{dailyCalorieTarget}} daily calories ({{dietType}} diet).
                       For each day, provide breakfast, lunch, and dinner with exact calories and macros (protein, carbs, fat in grams).
                       Format the response as a valid JSON object matching this C# class structure:

                       public class WeeklyMealPlan {
                           public List<MealDay> Days { get; set; }
                           public int TotalWeeklyCalories { get; set; }
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

        try
        {
            var messages = new List<Message>()
            {
                new Message(RoleType.User, prompt),
            };

            
            var parameters = new MessageParameters()
            {
                Messages = messages,
                MaxTokens = 4096,
                Model = AnthropicModels.Claude35Sonnet,
                Stream = false,
                Temperature = 0.7m,
            };

            var firstResult = await _anthropicClient.Messages.GetClaudeMessageAsync(parameters);

            firstResult.Message.ToString();

            return null;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to generate meal plan", ex);
        }
    }
}