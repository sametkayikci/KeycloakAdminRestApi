using System.ComponentModel.DataAnnotations;

namespace KeycloakAdminRestApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            await HandleExceptionAsync(context, e);
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
            HttpRequestException e => (int)e.StatusCode!.Value,
            ArgumentNullException => StatusCodes.Status400BadRequest,
            BadHttpRequestException e => e.StatusCode,
            UnauthorizedAccessException e => StatusCodes.Status401Unauthorized,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception)
    {
        return exception switch
        {
            BadHttpRequestException => "Bad Http Request Error",
            HttpRequestException => "Http Request Error",
            ArgumentNullException => "Argument Null Error",
            UnauthorizedAccessException => "Unauthorized Error",
            _ => "Server Error"
        };
    }

    private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
    {
        var errors = new Dictionary<string, string[]>();

        switch (exception)
        {
            case HttpRequestException dbException:
                errors["HttpRequestException"] = new[] { dbException.Message };
                break;
        }

        var innerException = exception.InnerException;
        var innerErrors = new List<string>();

        while (innerException != null)
        {
            innerErrors.Add(innerException.Message);
            innerException = innerException.InnerException;
        }

        if (innerErrors.Any())
        {
            errors!["InnerExceptions"] = innerErrors.ToArray();
        }

        return errors;
    }
}

public static class ExceptionHandlingExtensions
{
    public static void UseExceptionsHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}