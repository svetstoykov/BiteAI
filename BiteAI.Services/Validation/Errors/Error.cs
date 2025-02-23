using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Validation.Errors;

public class Error
{
    public string Message { get; }
    public ErrorType Type { get; }
    public string? Code { get; }

    public Error(string message, ErrorType type, string? code = null)
    {
        this.Message = message;
        this.Type = type;
        this.Code = code;
    }
    
    public static Error NotFound(string message, string? code = null) 
        => new Error(message, ErrorType.NotFound, code);

    public static Error Validation(string message, string? code = null) 
        => new Error(message, ErrorType.Validation, code);

    public static Error Unauthorized(string message, string? code = null) 
        => new Error(message, ErrorType.Unauthorized, code);

    public static Error BadRequest(string message, string? code = null) 
        => new Error(message, ErrorType.BadRequest, code);

    public static Error Conflict(string message, string? code = null) 
        => new Error(message, ErrorType.Conflict, code);

    public static Error InternalError(string message, string? code = null) 
        => new Error(message, ErrorType.InternalError, code);

    public static Error ExternalServiceError(string message, string? code = null) 
        => new Error(message, ErrorType.ExternalServiceError, code);

    public static Error InvalidOperation(string message, string? code = null) 
        => new Error(message, ErrorType.InvalidOperation, code);

    public static Error Timeout(string message, string? code = null) 
        => new Error(message, ErrorType.Timeout, code);
}