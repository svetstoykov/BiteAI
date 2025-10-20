namespace BiteAI.Infrastructure.Settings;

public class OpenRouterConfiguration
{
    public required string ApiKey { get; set; }
    public required string Model { get; set; }
    public string BaseUrl { get; set; } = "https://openrouter.ai/api/v1";
    public decimal Temperature { get; set; } = 0.1m;
    public int MaxTokens { get; set; } = 8192;
}
