using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Validation.Errors;

public class OperationError
{
    public string Message { get; }
    public ErrorType Type { get; }
    public string? Code { get; }

    public OperationError(string message, ErrorType type, string? code = null)
    {
        this.Message = message;
        this.Type = type;
        this.Code = code;
    }
    
    public static OperationError NotFound(string message, string? code = null) 
        => new OperationError(message, ErrorType.NotFound, code);

    public static OperationError Validation(string message, string? code = null) 
        => new OperationError(message, ErrorType.Validation, code);

    public static OperationError Unauthorized(string message, string? code = null) 
        => new OperationError(message, ErrorType.Unauthorized, code);

    public static OperationError BadRequest(string message, string? code = null) 
        => new OperationError(message, ErrorType.BadRequest, code);

    public static OperationError Conflict(string message, string? code = null) 
        => new OperationError(message, ErrorType.Conflict, code);

    public static OperationError InternalError(string message, string? code = null) 
        => new OperationError(message, ErrorType.InternalError, code);

    public static OperationError ExternalServiceError(string message, string? code = null) 
        => new OperationError(message, ErrorType.ExternalServiceError, code);

    public static OperationError InvalidOperation(string message, string? code = null) 
        => new OperationError(message, ErrorType.InvalidOperation, code);

    public static OperationError Timeout(string message, string? code = null) 
        => new OperationError(message, ErrorType.Timeout, code);
}