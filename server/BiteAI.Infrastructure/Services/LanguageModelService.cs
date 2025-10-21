using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using BiteAI.Infrastructure.Settings;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace BiteAI.Infrastructure.Services;

public class LanguageModelService : ILanguageModelService
{
    private readonly ChatClient _chatClient;
    private readonly OpenRouterConfiguration _config;
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public LanguageModelService(
        IOptions<OpenRouterConfiguration> configOptions,
        IMemoryCache memoryCache)
    {
        this._config = configOptions.Value;
        this._memoryCache = memoryCache;
        this._cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
            .SetPriority(CacheItemPriority.Normal);

        // Configure OpenAI client to use OpenRouter
        var clientOptions = new OpenAIClientOptions
        {
            Endpoint = new Uri(this._config.BaseUrl)
        };

        var openAiClient = new OpenAIClient(new ApiKeyCredential(this._config.ApiKey), clientOptions);
        this._chatClient = openAiClient.GetChatClient(this._config.Model);
    }

    public async Task<Result<TResponse?>> PromptAsync<TResponse>(string prompt, string systemPrompt, CancellationToken cancellationToken = default)
        where TResponse : class, new()
    {
        var cacheKey = $"AIService_Response_{typeof(TResponse).Name}_{prompt.GetHashCode()}";

        if (this._memoryCache.TryGetValue(cacheKey, out TResponse? cachedResult))
            return Result.Success(cachedResult);

        var result = await this.PromptOpenRouterAsync<TResponse>(prompt, systemPrompt, cancellationToken);
        if (result.IsFailure)
            return result;
        
        this._memoryCache.Set(cacheKey, result.Data, this._cacheOptions);

        return result;
    }

    private async Task<Result<TResponse?>> PromptOpenRouterAsync<TResponse>(string userPrompt, string systemPrompt, CancellationToken cancellationToken)
        where TResponse : class, new()
    {
        try
        {
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt)
            };

            var chatOptions = new ChatCompletionOptions
            {
                Temperature = (float)this._config.Temperature,
                MaxOutputTokenCount = this._config.MaxTokens,
                ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
            };

            var response = await this._chatClient.CompleteChatAsync(messages, chatOptions, cancellationToken);

            if (response?.Value?.Content == null || response.Value.Content.Count == 0)
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("The service returned no content."));

            var contentText = response.Value.Content[0].Text;
            var sanitized = SanitizeJsonOutput(contentText);

            if (string.IsNullOrWhiteSpace(sanitized))
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("The service returned an empty response."));

            var deserialized = JsonSerializer.Deserialize<TResponse>(sanitized, this._jsonOptions);
            return Result.Success(deserialized);
        }
        catch (ClientResultException ex)
        {
            return Result.Fail<TResponse?>(OperationError.ExternalServiceError($"OpenRouter error: {ex.Status} - {ex.Message}"));
        }
        catch (Exception)
        {
            return Result.Fail<TResponse?>(OperationError.ExternalServiceError("An error occurred while processing the OpenRouter response."));
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