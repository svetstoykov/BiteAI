using Anthropic.SDK;
using BiteAI.Infrastructure.Services;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Services;

namespace BiteAI.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICalorieService, CalorieService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IMealPlanningService, MealPlanningService>();
        services.AddScoped<AnthropicClient>(_ => new AnthropicClient(configuration["Anthropic:ApiKey"]));
        services.AddMemoryCache();

        return services;
    }
}