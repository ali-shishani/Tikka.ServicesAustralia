using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tikka.ServicesAustralia.Core.Models;

namespace Tikka.ServicesAustralia.Core.Middlewares;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IHostEnvironment env)
{
    /// <summary>
    /// Invoke the middleware.
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //var response = env.IsDevelopment()
            //    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace)
            //    : new AppException(context.Response.StatusCode, ex.Message, "Internal server error");

            var response = env.IsDevelopment()
                ? ApiResponse<object>.FailureResponse(context.Response.StatusCode, [new AppException(ex.Message, ex.StackTrace)])
                : ApiResponse<object>.FailureResponse(context.Response.StatusCode, [new AppException(ex.Message, "Internal server error")]);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}