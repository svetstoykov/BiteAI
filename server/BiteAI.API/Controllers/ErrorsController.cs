using BiteAI.API.Controllers.Base;
using BiteAI.API.Models.Errors;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

[AllowAnonymous]
public class OperationErrorsController : BaseApiController
{
    [HttpGet("not-found")]
    public IActionResult TestNotFound()
    {
        var result = Result.Fail(OperationError.NotFound("Resource not found", "TEST_404"));
        return this.ToErrorResult(result);
    }

    [HttpGet("validation-error")]
    public IActionResult TestValidation()
    {
        var result = Result.Fail(OperationError.Validation("Invalid input data", "VALIDATION_001"));
        return this.ToErrorResult(result);
    }

    [HttpGet("unauthorized")]
    public IActionResult TestUnauthorized()
    {
        var result = Result.Fail(OperationError.Unauthorized("Access denied", "AUTH_001"));
        return this.ToErrorResult(result);
    }

    [HttpGet("bad-request")]
    public IActionResult TestBadRequest()
    {
        var result = Result.Fail(OperationError.BadRequest("Bad request data", "BAD_REQ_001"));
        return this.ToErrorResult(result);
    }

    [HttpGet("conflict")]
    public IActionResult TestConflict()
    {
        var result = Result.Fail(OperationError.Conflict("Resource already exists", "CONFLICT_001"));
        return this.ToErrorResult(result);
    }

    [HttpGet("timeout")]
    public IActionResult TestTimeout()
    {
        var result = Result.Fail(OperationError.Timeout("Operation timed out", "TIMEOUT_001"));
        return this.ToErrorResult(result);
    }

    [HttpGet("external-error")]
    public IActionResult TestExternalOperationError()
    {
        var result = Result.Fail(OperationError.ExternalServiceError("External service failed", "EXT_ERR_001"));
        return this.ToErrorResult(result);
    }

    [HttpGet("internal-error")]
    public IActionResult TestInternalOperationError()
    {
        var result = Result.Fail(OperationError.InternalError("Something went wrong", "INT_ERR_001"));
        return this.ToErrorResult(result);
    }

    [HttpGet("invalid-operation")]
    public IActionResult TestInvalidOperationError()
    {
        var result = Result.Fail(OperationError.InvalidOperation("Something went wrong", "INVALID_OPERATION_001"));
        return this.ToErrorResult(result);
    }

    [HttpGet("success")]
    public IActionResult TestSuccess()
    {
        var result = Result.Success("Test success");

        return this.ToActionResult(result);
    }
    
    [HttpGet("success-data")]
    public IActionResult TestSuccessWithData()
    {
        var data = new TestData()
        {
            Id = 1,
            Name = "Test name",
            CreatedAt = DateTime.UtcNow
        };
        
        var result = Result.Success(data, "Test success");
        
        return this.ToActionResult(result);
    }
}