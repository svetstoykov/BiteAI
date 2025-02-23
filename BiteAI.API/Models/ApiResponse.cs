namespace BiteAI.API.Models;

public class ApiResponse<T>
{
    public ApiResponse(string message, T? data = default)
    {
        
    }
    
    public T? Data { get; set; }
    
    public string? Message { get; protected set; }
}