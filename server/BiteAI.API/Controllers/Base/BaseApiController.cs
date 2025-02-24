using BiteAI.API.Models;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    [NonAction] 
    protected IActionResult ToErrorResult(Result result)
    {
        var error = result.Errors.First();
        var response = new ApiResponse<object>(success: false, message: error.Message);

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
}