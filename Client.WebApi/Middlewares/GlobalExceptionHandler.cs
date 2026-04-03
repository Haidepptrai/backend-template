using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Client.WebApi.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var statusCode = StatusCodes.Status500InternalServerError;
        var title = "Server error";
        var detail = "Something went wrong, and we are investigating the issue";

        switch (exception)
        {
            case Application.Common.Exceptions.NotFoundException notFoundException:
                statusCode = StatusCodes.Status404NotFound;
                title = "Not Found";
                detail = notFoundException.Message;
                break;
            case Application.Common.Exceptions.BusinessValidationException businessException:
                statusCode = StatusCodes.Status400BadRequest;
                title = "Bad Request";
                detail = businessException.Message;
                break;
            case Application.Common.Exceptions.AlreadyExistsException alreadyExistsException:
                statusCode = StatusCodes.Status409Conflict;
                title = "Conflict";
                detail = alreadyExistsException.Message;
                break;
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
