using System.Diagnostics.CodeAnalysis;
using System.Net;
using Application.Exceptions;
using Newtonsoft.Json;

namespace UrlShortenerApi.Middleware;

[ExcludeFromCodeCoverage]
public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        context.Response.StatusCode = exception switch
        {
            InvalidCredentialsException _ => (int)HttpStatusCode.Unauthorized,
            BadRequestException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int) HttpStatusCode.NotFound,
            DuplicateException => (int) HttpStatusCode.Conflict,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var errorMessage = ConstructExceptionMessage(exception);
        
        return context.Response.WriteAsync(errorMessage);
    }

    private static string ConstructExceptionMessage(Exception exception)
    {
        if (exception is BadRequestException errorException)
        {
            return JsonConvert.SerializeObject(new
            {
                error = errorException.Errors
            });
        }

        return JsonConvert.SerializeObject(new
        {
            error = exception.Message
        });
    }
}