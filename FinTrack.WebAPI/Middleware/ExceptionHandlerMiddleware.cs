using System.Net;
using System.Text.Json;

namespace FinTrack.WebAPI.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (InvalidOperationException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                httpContext.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(ex.Message);
                await httpContext.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "text/plain";
                await httpContext.Response.WriteAsync($"An internal server error occurred: {ex.Message}");
            }
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
