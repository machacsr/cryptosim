using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSim.Services.Exceptions;

[Serializable]
public class BadRequestException(string error, string message) : Exception(message)
{
    public string Error { get; } = error;

    public override string Message { get; } = message;
}

public class BadRequestExceptionHandlerImpl(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadRequestException problemException)
        {
            return true;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = problemException.Error,
            Detail = problemException.Message,
            Type = "Bad Request"
        };

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            }
        );
    }
}