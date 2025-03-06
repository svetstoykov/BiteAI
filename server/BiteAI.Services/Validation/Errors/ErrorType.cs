namespace BiteAI.Services.Validation.Errors;

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
    Timeout,
    UnprocessableEntity,
}