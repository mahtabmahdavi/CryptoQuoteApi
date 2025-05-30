using CryptoQuoteApi.Application.Exceptions;
using FluentValidation;
using System.Net;

namespace CryptoQuoteApi.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, cannot write error response.");
                throw;
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var statusCode = (int)HttpStatusCode.InternalServerError;
        object errorResponse;

        switch (exception)
        {
            case ApiException apiException:
                statusCode = apiException.StatusCode;
                errorResponse = new
                {
                    Error = new
                    {
                        Code = apiException.ErrorCode,
                        Message = apiException.Message,
                        Details = _env.IsDevelopment() ? exception.Message : null
                    }
                };
                break;

            case ValidationException validationException:
                statusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new
                {
                    Error = new
                    {
                        Code = "VALIDATION_ERROR",
                        Message = "Validation failed",
                        Details = validationException.Errors.Select(e => new
                        {
                            Property = e.PropertyName,
                            Message = e.ErrorMessage
                        })
                    }
                };
                break;

            default:
                _logger.LogError(exception, "An unhandled exception occurred");
                errorResponse = new
                {
                    Error = new
                    {
                        Code = "INTERNAL_ERROR",
                        Message = "An error occurred while processing your request.",
                        Details = _env.IsDevelopment() ? exception.Message : null,
                        StackTrace = _env.IsDevelopment() ? exception.StackTrace : null
                    }
                };
                break;
        }

        response.StatusCode = statusCode;

        await response.WriteAsJsonAsync(errorResponse);
    }
}
