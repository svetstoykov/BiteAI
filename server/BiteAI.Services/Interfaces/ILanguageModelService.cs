using System.Text.Json;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface ILanguageModelService
{
    Task<Result<TResponse?>> PromptAsync<TResponse>(string userPrompt, string systemPrompt, CancellationToken cancellationToken = default)
        where TResponse : class, new();
}