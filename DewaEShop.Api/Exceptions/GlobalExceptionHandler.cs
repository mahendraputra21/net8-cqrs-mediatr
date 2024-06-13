using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace cqrs_mediatr.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            if (exception is FluentValidation.ValidationException fluentException)
            {
                problemDetails.Title = "one or more validation errors occurred.";
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                List<string> validationErrors = new();
                foreach (var error in fluentException.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                problemDetails.Extensions.Add("errors", validationErrors);

            }

            else
                problemDetails.Title = exception.Message;


            logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);

            problemDetails.Status = httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
