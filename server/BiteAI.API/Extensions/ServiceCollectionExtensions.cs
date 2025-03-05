using Anthropic.SDK;
using BiteAI.Infrastructure.Services;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace BiteAI.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICalorieService, CalorieService>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddAnthropicService(configuration);

        return services;
    }
    
    private static IServiceCollection AddAnthropicService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AnthropicClient>(_ => new AnthropicClient(configuration["Anthropic:ApiKey"]));
        services.AddScoped<IMealPlanningService, MealPlanningService>();

        return services;
    }
}