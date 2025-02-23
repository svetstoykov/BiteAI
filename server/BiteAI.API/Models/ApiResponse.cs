namespace BiteAI.API.Models;

public class ApiResponse<T>
{
    public ApiResponse(string message, T? data = default)
    {
        this.Message = message ?? throw new ArgumentNullException(nameof(message));
        this.Data = data;
    }

    public T? Data { get; set; }

    public string? Message { get; protected set; }
}