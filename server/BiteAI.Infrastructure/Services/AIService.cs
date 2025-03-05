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
            var response = await this._anthropicClient.Messages.GetClaudeMessageAsync(parameters, cancellationToken);

            if (response == null)
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("Response was null"));

            var output = response.Message?.ToString();
            if (string.IsNullOrEmpty(output))
                return Result.Fail<TResponse?>(OperationError.ExternalServiceError("Response was empty"));
            
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