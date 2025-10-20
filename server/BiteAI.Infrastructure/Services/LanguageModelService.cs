using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BiteAI.Infrastructure.Settings;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BiteAI.Infrastructure.Services;

public class LanguageModelService : ILanguageModelService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenRouterConfiguration _config;
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public LanguageModelService(
        IHttpClientFactory httpClientFactory,
        IOptions<OpenRouterConfiguration> configOptions,
        IMemoryCache memoryCache)
    {
        this._httpClientFactory = httpClientFactory;
        this._config = configOptions.Value;
        this._memoryCache = memoryCache;
        this._cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
            .SetPriority(CacheItemPriority.Normal);
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
            var client = this._httpClientFactory.CreateClient(nameof(LanguageModelService));
            client.BaseAddress = new Uri(string.IsNullOrWhiteSpace(this._config.BaseUrl) ? "https://openrouter.ai/api/v1" : this._config.BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._config.ApiKey);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var payload = new
            {
                model = this._config.Model,
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                temperature = this._config.Temperature,
                max_tokens = this._config.MaxTokens,
                response_format = new { type = "json_object" }
            };

            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await client.PostAsync("/chat/completions", content, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var errText = await response.Content.ReadAsStringAsync(cancellationToken);
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError($"OpenRouter error: {(int)response.StatusCode} - {errText}"));
            }

            var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

            // Very small DTO to parse content quickly
            using var doc = JsonDocument.Parse(responseText);
            var root = doc.RootElement;
            var choices = root.GetProperty("choices");
            if (choices.GetArrayLength() == 0)
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("The service returned no choices."));

            var contentText = choices[0].GetProperty("message").GetProperty("content").GetString();
            var sanitized = SanitizeJsonOutput(contentText);

            if (string.IsNullOrWhiteSpace(sanitized))
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("The service returned an empty response."));

            var deserialized = JsonSerializer.Deserialize<TResponse>(sanitized, this._jsonOptions);
            return Result.Success(deserialized);
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