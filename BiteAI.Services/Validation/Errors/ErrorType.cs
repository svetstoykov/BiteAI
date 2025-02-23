namespace BiteAI.Services.Validation.Result;

public enum ErrorType
{
    None,
    NotFound,
    Validation,
    Unauthorized,
    BadRequest,
    Conflict,
    InternalError,
    ExternalServiceError,
    InvalidOperation,
    Timeout
}