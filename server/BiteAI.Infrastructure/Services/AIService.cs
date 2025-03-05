using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Anthropic.SDK;
using Anthropic.SDK.Common;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using GenerativeAI;
using GenerativeAI.Core;
using GenerativeAI.Types;
using Microsoft.Extensions.Configuration;
using Tool = Anthropic.SDK.Common.Tool;

namespace BiteAI.Infrastructure.Services;

public class AIService : IAIService
{
    private readonly AnthropicClient _anthropicClient;
    private readonly IConfiguration _configuration;

    public AIService(AnthropicClient anthropicClient, IConfiguration configuration)
    {
        this._anthropicClient = anthropicClient;
        this._configuration = configuration;
    }

    public async Task<Result<TResponse?>> PromptAsync<TResponse>(string prompt, CancellationToken cancellationToken = default)
        where TResponse : class
    {
        return await this.PromptClaudeAsync<TResponse>(prompt, cancellationToken);
    }

    private async Task<Result<TResponse?>> PromptClaudeAsync<TResponse>(string prompt, CancellationToken cancellationToken = default)
    {
        var messages = new List<Message>
        {
            new(RoleType.User, prompt)
        };

        var parameters = new MessageParameters()
        {
            Messages = messages,
            MaxTokens = 8192,
            Model = AnthropicModels.Claude35Haiku,
            Stream = false,
            Temperature = 0.7m,
            System = [new SystemMessage("Response should be exactly a single JSON output, nothing more.")]
        };

        TResponse? responseObject = default;
        try
        {
            // var response = await this._anthropicClient.Messages.GetClaudeMessageAsync(parameters, cancellationToken);
            //
            // if (response == null)
            //     return Result.Fail<TResponse?>(OperationError.ExternalServiceError("Response was null"));
            //
            // var output = response.Message?.ToString();
            // if (string.IsNullOrEmpty(output))
            //     return Result.Fail<TResponse?>(OperationError.ExternalServiceError("Response was empty"));

            var output =
                "{\n  \"Meals\" : [ {\n    \"DayNumber\" : 1,\n    \"Meals\" : [ {\n      \"Name\" : \"Greek Yogurt Parfait\",\n      \"Recipe\" : \"Layer Greek yogurt with granola, mixed berries, and honey\",\n      \"Calories\" : 450,\n      \"MealType\" : \"Breakfast\",\n      \"ProteinInGrams\" : 25,\n      \"CarbsInGrams\" : 55,\n      \"FatInGrams\" : 15\n    }, {\n      \"Name\" : \"Quinoa Black Bean Salad\",\n      \"Recipe\" : \"Mixed quinoa, black beans, roasted vegetables, olive oil dressing\",\n      \"Calories\" : 550,\n      \"MealType\" : \"Lunch\",\n      \"ProteinInGrams\" : 22,\n      \"CarbsInGrams\" : 75,\n      \"FatInGrams\" : 20\n    }, {\n      \"Name\" : \"Vegetable Tofu Stir Fry\",\n      \"Recipe\" : \"Tofu, mixed vegetables, brown rice, soy sauce\",\n      \"Calories\" : 600,\n      \"MealType\" : \"Dinner\",\n      \"ProteinInGrams\" : 35,\n      \"CarbsInGrams\" : 65,\n      \"FatInGrams\" : 22\n    }, {\n      \"Name\" : \"Almonds and Apple\",\n      \"Recipe\" : \"Handful of almonds with fresh apple\",\n      \"Calories\" : 250,\n      \"MealType\" : \"Snack\",\n      \"ProteinInGrams\" : 8,\n      \"CarbsInGrams\" : 25,\n      \"FatInGrams\" : 15\n    } ]\n  }, {\n    \"DayNumber\" : 2,\n    \"Meals\" : [ {\n      \"Name\" : \"Spinach Mushroom Frittata\",\n      \"Recipe\" : \"Egg whites, spinach, mushrooms, feta cheese\",\n      \"Calories\" : 400,\n      \"MealType\" : \"Breakfast\",\n      \"ProteinInGrams\" : 30,\n      \"CarbsInGrams\" : 15,\n      \"FatInGrams\" : 25\n    }, {\n      \"Name\" : \"Mediterranean Chickpea Wrap\",\n      \"Recipe\" : \"Whole wheat wrap, hummus, roasted chickpeas, cucumber, tomato\",\n      \"Calories\" : 525,\n      \"MealType\" : \"Lunch\",\n      \"ProteinInGrams\" : 20,\n      \"CarbsInGrams\" : 65,\n      \"FatInGrams\" : 18\n    }, {\n      \"Name\" : \"Lentil Curry with Cauliflower Rice\",\n      \"Recipe\" : \"Red lentils, coconut milk, spices, cauliflower rice\",\n      \"Calories\" : 650,\n      \"MealType\" : \"Dinner\",\n      \"ProteinInGrams\" : 35,\n      \"CarbsInGrams\" : 70,\n      \"FatInGrams\" : 25\n    }, {\n      \"Name\" : \"Greek Yogurt with Honey\",\n      \"Recipe\" : \"Low-fat Greek yogurt with honey and chia seeds\",\n      \"Calories\" : 250,\n      \"MealType\" : \"Snack\",\n      \"ProteinInGrams\" : 15,\n      \"CarbsInGrams\" : 30,\n      \"FatInGrams\" : 8\n    } ]\n  }, {\n    \"DayNumber\" : 3,\n    \"Meals\" : [ {\n      \"Name\" : \"Overnight Chia Oats\",\n      \"Recipe\" : \"Chia seeds, rolled oats, almond milk, berries\",\n      \"Calories\" : 425,\n      \"MealType\" : \"Breakfast\",\n      \"ProteinInGrams\" : 22,\n      \"CarbsInGrams\" : 55,\n      \"FatInGrams\" : 16\n    }, {\n      \"Name\" : \"Vegetable Protein Bowl\",\n      \"Recipe\" : \"Roasted sweet potato, edamame, kale, tahini dressing\",\n      \"Calories\" : 575,\n      \"MealType\" : \"Lunch\",\n      \"ProteinInGrams\" : 25,\n      \"CarbsInGrams\" : 65,\n      \"FatInGrams\" : 22\n    }, {\n      \"Name\" : \"Stuffed Bell Peppers\",\n      \"Recipe\" : \"Quinoa, black beans, cheese, marinara sauce\",\n      \"Calories\" : 600,\n      \"MealType\" : \"Dinner\",\n      \"ProteinInGrams\" : 30,\n      \"CarbsInGrams\" : 70,\n      \"FatInGrams\" : 20\n    }, {\n      \"Name\" : \"Apple with Almond Butter\",\n      \"Recipe\" : \"Fresh apple slices with natural almond butter\",\n      \"Calories\" : 250,\n      \"MealType\" : \"Snack\",\n      \"ProteinInGrams\" : 7,\n      \"CarbsInGrams\" : 25,\n      \"FatInGrams\" : 16\n    } ]\n  }, {\n    \"DayNumber\" : 4,\n    \"Meals\" : [ {\n      \"Name\" : \"Tofu Scramble\",\n      \"Recipe\" : \"Scrambled tofu, bell peppers, spinach, whole grain toast\",\n      \"Calories\" : 425,\n      \"MealType\" : \"Breakfast\",\n      \"ProteinInGrams\" : 28,\n      \"CarbsInGrams\" : 35,\n      \"FatInGrams\" : 22\n    }, {\n      \"Name\" : \"Greek Salad with Feta\",\n      \"Recipe\" : \"Mixed greens, cucumber, tomato, olives, feta cheese\",\n      \"Calories\" : 525,\n      \"MealType\" : \"Lunch\",\n      \"ProteinInGrams\" : 20,\n      \"CarbsInGrams\" : 25,\n      \"FatInGrams\" : 40\n    }, {\n      \"Name\" : \"Vegetable Lasagna\",\n      \"Recipe\" : \"Zucchini, spinach, ricotta, marinara sauce\",\n      \"Calories\" : 650,\n      \"MealType\" : \"Dinner\",\n      \"ProteinInGrams\" : 35,\n      \"CarbsInGrams\" : 55,\n      \"FatInGrams\" : 35\n    }, {\n      \"Name\" : \"Protein Smoothie\",\n      \"Recipe\" : \"Plant protein, banana, spinach, almond milk\",\n      \"Calories\" : 250,\n      \"MealType\" : \"Snack\",\n      \"ProteinInGrams\" : 20,\n      \"CarbsInGrams\" : 30,\n      \"FatInGrams\" : 6\n    } ]\n  }, {\n    \"DayNumber\" : 5,\n    \"Meals\" : [ {\n      \"Name\" : \"Avocado Toast\",\n      \"Recipe\" : \"Whole grain bread, mashed avocado, cherry tomatoes, seeds\",\n      \"Calories\" : 450,\n      \"MealType\" : \"Breakfast\",\n      \"ProteinInGrams\" : 15,\n      \"CarbsInGrams\" : 45,\n      \"FatInGrams\" : 28\n    }, {\n      \"Name\" : \"Buddha Bowl\",\n      \"Recipe\" : \"Roasted chickpeas, quinoa, kale, tahini dressing\",\n      \"Calories\" : 550,\n      \"MealType\" : \"Lunch\",\n      \"ProteinInGrams\" : 25,\n      \"CarbsInGrams\" : 65,\n      \"FatInGrams\" : 20\n    }, {\n      \"Name\" : \"Eggplant Parmesan\",\n      \"Recipe\" : \"Baked eggplant, marinara, mozzarella, basil\",\n      \"Calories\" : 625,\n      \"MealType\" : \"Dinner\",\n      \"ProteinInGrams\" : 30,\n      \"CarbsInGrams\" : 45,\n      \"FatInGrams\" : 35\n    }, {\n      \"Name\" : \"Cottage Cheese with Berries\",\n      \"Recipe\" : \"Low-fat cottage cheese, mixed berries\",\n      \"Calories\" : 250,\n      \"MealType\" : \"Snack\",\n      \"ProteinInGrams\" : 20,\n      \"CarbsInGrams\" : 25,\n      \"FatInGrams\" : 8\n    } ]\n  }, {\n    \"DayNumber\" : 6,\n    \"Meals\" : [ {\n      \"Name\" : \"Protein Pancakes\",\n      \"Recipe\" : \"Protein powder, banana, egg whites, maple syrup\",\n      \"Calories\" : 425,\n      \"MealType\" : \"Breakfast\",\n      \"ProteinInGrams\" : 35,\n      \"CarbsInGrams\" : 50,\n      \"FatInGrams\" : 12\n    }, {\n      \"Name\" : \"Caprese Sandwich\",\n      \"Recipe\" : \"Whole grain bread, fresh mozzarella, tomato, basil\",\n      \"Calories\" : 525,\n      \"MealType\" : \"Lunch\",\n      \"ProteinInGrams\" : 22,\n      \"CarbsInGrams\" : 55,\n      \"FatInGrams\" : 25\n    }, {\n      \"Name\" : \"Vegetable Risotto\",\n      \"Recipe\" : \"Arborio rice, mushrooms, asparagus, parmesan\",\n      \"Calories\" : 650,\n      \"MealType\" : \"Dinner\",\n      \"ProteinInGrams\" : 25,\n      \"CarbsInGrams\" : 85,\n      \"FatInGrams\" : 22\n    }, {\n      \"Name\" : \"Trail Mix\",\n      \"Recipe\" : \"Mixed nuts, seeds, dried fruit\",\n      \"Calories\" : 250,\n      \"MealType\" : \"Snack\",\n      \"ProteinInGrams\" : 10,\n      \"CarbsInGrams\" : 20,\n      \"FatInGrams\" : 18\n    } ]\n  }, {\n    \"DayNumber\" : 7,\n    \"Meals\" : [ {\n      \"Name\" : \"Vegetable Egg White Omelet\",\n      \"Recipe\" : \"Egg whites, spinach, mushrooms, feta cheese\",\n      \"Calories\" : 400,\n      \"MealType\" : \"Breakfast\",\n      \"ProteinInGrams\" : 35,\n      \"CarbsInGrams\" : 15,\n      \"FatInGrams\" : 20\n    }, {\n      \"Name\" : \"Mexican Bean Salad\",\n      \"Recipe\" : \"Black beans, corn, tomatoes, avocado, lime dressing\",\n      \"Calories\" : 525,\n      \"MealType\" : \"Lunch\",\n      \"ProteinInGrams\" : 22,\n      \"CarbsInGrams\" : 60,\n      \"FatInGrams\" : 22\n    }, {\n      \"Name\" : \"Spinach Ricotta Cannelloni\",\n      \"Recipe\" : \"Pasta tubes, spinach, ricotta, marinara sauce\",\n      \"Calories\" : 625,\n      \"MealType\" : \"Dinner\",\n      \"ProteinInGrams\" : 30,\n      \"CarbsInGrams\" : 70,\n      \"FatInGrams\" : 25\n    }, {\n      \"Name\" : \"Protein Energy Balls\",\n      \"Recipe\" : \"Dates, nuts, protein powder, coconut\",\n      \"Calories\" : 250,\n      \"MealType\" : \"Snack\",\n      \"ProteinInGrams\" : 12,\n      \"CarbsInGrams\" : 25,\n      \"FatInGrams\" : 15\n    } ]\n  } ]\n}";
            
            responseObject = JsonSerializer.Deserialize<TResponse>(output);
        }
        catch (Exception e)
        {
            return Result.Fail<TResponse?>(OperationError.ExternalServiceError(e.Message));
        }

        return Result.Success(responseObject);
    }

    private async Task<TResponse> PromptGeminiAsync<TResponse>(string message, CancellationToken cancellationToken) where TResponse : class
    {
        var googleAI = new GenerativeModel(this._configuration["Google:ApiKey"], "gemini-2.0-flash");

        var request = new GenerateContentRequest();
        request.AddText(message);
        request.UseJsonMode<TResponse>();

        var response = await googleAI.GenerateContentAsync(request, cancellationToken);

        return null;
    }
}