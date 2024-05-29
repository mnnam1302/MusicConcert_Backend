using Application.Exceptions;
using Domain.Exceptions;
using System.Text.Json;

namespace API.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new
        {
            title = GetTitle(exception),
            status = statusCode,
            detail = exception.Message,
            errors = GetErrors(exception)
        };

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            // Category
            CategoryException.CategoryNotFoundException => StatusCodes.Status404NotFound,

            // Event
            EventException.EventNotFoundException => StatusCodes.Status404NotFound,
            EventException.EventFieldException => StatusCodes.Status400BadRequest,
            EventException.EventTypeException => StatusCodes.Status400BadRequest,

            // Firebase
            FirebaseException.FireBaseAuthenticateException => StatusCodes.Status401Unauthorized,

            // Domain
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,

            FormatException => StatusCodes.Status422UnprocessableEntity,
            InvalidOperationException => StatusCodes.Status500InternalServerError,

            // Tường hợp mặc định, nếu không có th nào map ở trên thì trả về 500
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception) =>
        exception switch
        {
            DomainException applicationException => applicationException.Title,
            _ => "Server error"
        };

    private static IReadOnlyCollection<Application.Exceptions.ValidationError> GetErrors(Exception exception)
    {
        IReadOnlyCollection<Application.Exceptions.ValidationError> errors = null;

        if (exception is Application.Exceptions.ValidationException validationException)
        {
            errors = validationException.Errors;
        }

        return errors;
    }
}