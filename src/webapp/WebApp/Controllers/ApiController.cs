using Microsoft.AspNetCore.Mvc;
using WebApp.Domain.Common.Errors;
using WebApp.Domain.Common.Results;

namespace WebApp.Api.Controllers;

public abstract class ApiController : ControllerBase
{
    protected IActionResult HandleResult(Domain.Common.Results.Interfaces.IResult result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return MapErrorToResponse(result.Error);
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return MapErrorToResponse(result.Error);
    }

    private IActionResult MapErrorToResponse(Error error)
    {
        return error.Type switch
        {
            ErrorType.NotFound => NotFound(new { error = error.Code, message = error.Message }),
            ErrorType.Validation => BadRequest(new { error = error.Code, message = error.Message }),
            ErrorType.Conflict => Conflict(new { error = error.Code, message = error.Message }),
            ErrorType.Unauthorized => Unauthorized(new { error = error.Code, message = error.Message }),
            ErrorType.Forbidden => Forbid(),
            _ => StatusCode(500, new { error = error.Code, message = error.Message })
        };
    }
}
