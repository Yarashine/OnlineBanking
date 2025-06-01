using System.Net;
using System.Text.Json;
using UserService.Domain.Exceptions;

namespace UserService.API.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate _next,
    ILogger<ExceptionHandlingMiddleware> _logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла необработанная ошибка");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var statusCode = exception switch
        {
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            BadRequestException => HttpStatusCode.BadRequest,
            AlreadyExistsException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };

        response.StatusCode = (int)statusCode;

        var errorResponse = new
        {
            message = exception.Message,
            statusCode = response.StatusCode,
            errorType = exception.GetType().Name,
            StackTrace = context.RequestServices.GetService<IWebHostEnvironment>().IsDevelopment()
                ? exception.StackTrace
                : null,
        };

        return response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}