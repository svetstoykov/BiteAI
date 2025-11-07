using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using BiteAI.Infrastructure.Settings;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace BiteAI.Infrastructure.Services;

public class LanguageModelService : ILanguageModelService
{
    private readonly ChatClient _chatClient;
    private readonly OpenRouterConfiguration _config;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public LanguageModelService(
        IOptions<OpenRouterConfiguration> configOptions)
    {
        this._config = configOptions.Value;
        var clientOptions = new OpenAIClientOptions
        {
            Endpoint = new Uri(this._config.BaseUrl)
        };

        var openAiClient = new OpenAIClient(new ApiKeyCredential(this._config.ApiKey), clientOptions);
        this._chatClient = openAiClient.GetChatClient(this._config.Model);
    }

    public async Task<Result<TResponse?>> PromptAsync<TResponse>(string userPrompt, string systemPrompt, CancellationToken cancellationToken)
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