using System.Net;
using System.Text.Json;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middleware;

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
        catch (ValidationAppException ex)
        {
            await WriteResponse(context, HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail(ex.Errors.Select(e => new ApiError(e.Field, e.Message))));
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            await WriteResponse(context, HttpStatusCode.NotFound, ApiResponse<object>.Fail(ex.Message));
        }
        catch (DependencyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Dependency service check failed");
            await WriteResponse(context, HttpStatusCode.BadRequest, ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteResponse(context, HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail("خطای داخلی سرور رخ داده است."));
        }
    }

    private static Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object payload)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
