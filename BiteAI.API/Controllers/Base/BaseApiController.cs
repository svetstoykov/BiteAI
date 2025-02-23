using BiteAI.API.Models;
using BiteAI.Services.Validation.Result;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    public IActionResult ToErrorResult(Result result)
    {
        var error = result.Errors.First();
        var response = new ApiResponse<object>(error.Message);

        return error.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(error),
            ErrorType.Validation => new BadRequestObjectResult(error),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(error),
            ErrorType.BadRequest => new BadRequestObjectResult(error),
            ErrorType.Conflict => new ConflictObjectResult(error),
            ErrorType.Timeout => new StatusCodeResult(StatusCodes.Status504GatewayTimeout),
            ErrorType.ExternalServiceError => new StatusCodeResult(StatusCodes.Status502BadGateway),
            _ => new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}