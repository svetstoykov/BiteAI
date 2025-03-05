using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IAIService
{
    Task<Result<TResponse?>> PromptAsync<TResponse>(string prompt,  CancellationToken cancellationToken = default)
        where TResponse : class;
}