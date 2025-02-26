namespace BiteAI.API.Models;

public sealed class ApiResponse<T>
{
    public ApiResponse(bool success, string? message = null, T? data = default)
    {
        this.Message = message;
        this.Data = data;
        this.Success = success;
    }

    public T? Data { get; private set; }

    public string? Message { get; private set; }
    
    public bool Success { get; private set; }
}