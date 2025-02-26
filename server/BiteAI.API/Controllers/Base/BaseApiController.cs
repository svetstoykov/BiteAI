using BiteAI.API.Models;
using BiteAI.API.Models.DTOs.Errors;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    [NonAction]
    protected IActionResult ToActionResult(Result result) 
        => result.IsFailure ? this.ToErrorResult(result) : this.ToSuccessResult(result);
    
    [NonAction]
    protected IActionResult ToActionResult<T>(Result<T> result) 
        => result.IsFailure ? this.ToErrorResult(result) : this.ToSuccessResult(result);

    [NonAction] 
    protected IActionResult ToErrorResult(Result result)
    {
        var error = result.Errors.First();
        var response = new ApiResponse<ErrorDetail>(
            success: false, 
            message: error.Message,
            data: new ErrorDetail
            {
                Code = error.Code,
                Type = error.Type.ToString()
            });

        return error.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(response),
            ErrorType.Validation => new BadRequestObjectResult(response),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(response),
            ErrorType.BadRequest => new BadRequestObjectResult(response),
            ErrorType.Conflict => new ConflictObjectResult(response),
            ErrorType.Timeout => new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status408RequestTimeout
            },
            _ => new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }

    [NonAction]
    protected IActionResult ToSuccessResult<T>(Result<T> result)
    {
        return this.Ok(new ApiResponse<T>(
            success: true,
            message: result.SuccessMessage ?? "Success",
            data: result.Data
        ));
    }
    
    [NonAction]
    protected IActionResult ToSuccessResult(Result result)
    {
        return this.Ok(new ApiResponse<object>(
            success: true,
            message: result.SuccessMessage ?? "Success"
        ));
    }
}