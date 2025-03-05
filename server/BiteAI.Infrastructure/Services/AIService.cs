using System.Text.Json;
using System.Text.Json.Serialization;
using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using GenerativeAI;
using GenerativeAI.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace BiteAI.Infrastructure.Services;

public class AIService : IAIService
{
    private readonly AnthropicClient _anthropicClient;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public AIService(
        AnthropicClient anthropicClient, 
        IConfiguration configuration,
        IMemoryCache memoryCache)
    {
        this._anthropicClient = anthropicClient;
        this._configuration = configuration;
        this._memoryCache = memoryCache;
        this._cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
            .SetPriority(CacheItemPriority.Normal);
    }

    public async Task<Result<TResponse?>> PromptAsync<TResponse>(string prompt, CancellationToken cancellationToken = default)
        where TResponse : class, new()
    {
        var cacheKey = $"AIService_Response_{typeof(TResponse).Name}_{prompt.GetHashCode()}";

        if (this._memoryCache.TryGetValue(cacheKey, out TResponse? cachedResult))
            return Result.Success(cachedResult);

        var result = await this.PromptGeminiAsync<TResponse>(prompt, cancellationToken);
        if (result.IsFailure)
            return result;
        
        this._memoryCache.Set(cacheKey, result.Data, this._cacheOptions);

        return result;
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
            Temperature = 0.8m,
            System = [new SystemMessage("Response should be exactly a single JSON output, nothing more.")]
        };

        TResponse? responseObject = default;
        try
        {
            var response = await this._anthropicClient.Messages.GetClaudeMessageAsync(parameters, cancellationToken);

            if (response == null)
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("Response was null"));

            var output = response.Message?.ToString();
            if (string.IsNullOrEmpty(output))
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("Response was empty"));

            responseObject = JsonSerializer.Deserialize<TResponse>(output, this._jsonOptions);
        }
        catch (Exception e)
        {
            return Result.Fail<TResponse?>(OperationError.ExternalServiceError("Failed to parse response!"));
        }

        return Result.Success(responseObject);
    }

    private async Task<Result<TResponse?>> PromptGeminiAsync<TResponse>(string message, CancellationToken cancellationToken) 
        where TResponse : class, new()
    {
        try
        {
            var generativeModel = new GenerativeModel(this._configuration["Google:ApiKey"], "gemini-2.0-flash-lite");

            var request = new GenerateContentRequest();
            request.AddText(message);
            
            var response = await generativeModel.GenerateContentAsync(request, cancellationToken);
            
            if (response == null)
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("No response received from the external service."));
            
            var text = SanitizeJsonOutput(response.Text());

            if (string.IsNullOrEmpty(text))
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("The service returned an empty response."));

            var deserialized = JsonSerializer.Deserialize<TResponse>(text, this._jsonOptions);

            return Result.Success(deserialized);
        }
        catch (Exception e)
        {
            return Result.Fail<TResponse?>(OperationError.ExternalServiceError("An error occurred while processing the response."));
        }
    }

    private static string SanitizeJsonOutput(string? response)
    {
        if (string.IsNullOrEmpty(response))
            return string.Empty;

        var startIndex = response.IndexOf('{');
        var endIndex = response.LastIndexOf('}');

        if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
            return response.Substring(startIndex, endIndex - startIndex + 1).Trim();

        return response;
    }
}