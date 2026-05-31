using System.Net;
using System.Text.Json;
using ShopManagementSystem.Application.Common;
using ShopManagementSystem.Application.Exceptions;
using AppValidationException = ShopManagementSystem.Application.Exceptions.ValidationException;

namespace ShopManagementSystem.API.Middleware;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            NotFoundException notFound => (HttpStatusCode.NotFound, notFound.Message, (IEnumerable<string>?)null),
            AppValidationException validation => (HttpStatusCode.BadRequest, validation.Message, validation.Errors),
            BusinessRuleException business => (HttpStatusCode.BadRequest, business.Message, (IEnumerable<string>?)null),
            FluentValidation.ValidationException fluentValidation => (
                HttpStatusCode.BadRequest,
                "Validation failed.",
                fluentValidation.Errors.Select(e => e.ErrorMessage)),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", (IEnumerable<string>?)null)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.Fail(message, errors);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(json);
    }
}
