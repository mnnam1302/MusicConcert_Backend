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
            // CustomerInfo
            CustomerInfoException.CustomerInfoAlreadyExistsException => StatusCodes.Status400BadRequest,
            CustomerInfoException.CustomerInfoNotFoundException => StatusCodes.Status404NotFound,

            // TicketInfo
            TicketInfoException.TicketInfoAlreadyExistsException => StatusCodes.Status400BadRequest,
            TicketInfoException.TicketInfoNotFoundException => StatusCodes.Status404NotFound,
            TicketInfoException.TicketInfoNotExistsingException => StatusCodes.Status404NotFound,

            // Order
            OrderException.OrderFieldException => StatusCodes.Status400BadRequest,
            OrderException.OrderNotFoundException => StatusCodes.Status404NotFound,
            OrderException.OrderNotBelongCustomerException => StatusCodes.Status400BadRequest,

            //OrderDetails
            OrderDetailsException.OrderDetailsWithQuantityException => StatusCodes.Status400BadRequest,

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