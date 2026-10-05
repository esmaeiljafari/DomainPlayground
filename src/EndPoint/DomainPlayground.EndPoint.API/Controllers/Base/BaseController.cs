using DomainPlayground.Core.Application.Common.Interfaces;
using DomainPlayground.SharedKernel.Errors;
using DomainPlayground.SharedKernel.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DomainPlayground.EndPoint.API.Controllers.Base;

[ApiController]
public abstract class BaseController : ControllerBase
{
    private IErrorLocalizer? _localizer;
    private IErrorLocalizer Localizer =>
        _localizer ??= HttpContext.RequestServices.GetRequiredService<IErrorLocalizer>();

    private ICurrentUser? _currentUser;
    protected ICurrentUser CurrentUser =>
        _currentUser ??= HttpContext.RequestServices.GetRequiredService<ICurrentUser>();

    protected IActionResult HandleResult(Result result) =>
        result.IsSuccess ? NoContent() : ErrorResult(result.Error);

    protected IActionResult HandleResult<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : ErrorResult(result.Error);

    private ObjectResult ErrorResult(Error error)
    {
        var status = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status400BadRequest
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Detail = Localizer.Localize(error)
        };
        problem.Extensions["code"] = error.Code;

        return new ObjectResult(problem) { StatusCode = status };
    }
}