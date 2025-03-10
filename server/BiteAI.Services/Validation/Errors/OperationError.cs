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
        => new(message, ErrorType.NotFound, code);

    public static OperationError Validation(string message, string? code = null) 
        => new(message, ErrorType.Validation, code);

    public static OperationError Unauthorized(string message, string? code = null) 
        => new(message, ErrorType.Unauthorized, code);

    public static OperationError BadRequest(string message, string? code = null) 
        => new(message, ErrorType.BadRequest, code);

    public static OperationError Conflict(string message, string? code = null) 
        => new(message, ErrorType.Conflict, code);

    public static OperationError InternalError(string message, string? code = null) 
        => new(message, ErrorType.InternalError, code);

    public static OperationError ExternalServiceError(string message, string? code = null) 
        => new(message, ErrorType.ExternalServiceError, code);

    public static OperationError InvalidOperation(string message, string? code = null) 
        => new(message, ErrorType.InvalidOperation, code);

    public static OperationError Timeout(string message, string? code = null) 
        => new(message, ErrorType.Timeout, code);
    
    public static OperationError UnprocessableEntity(string message, string? code = null) 
        => new(message, ErrorType.UnprocessableEntity, code);
}