namespace FinTrack.WebAPI.Middleware
{
    public class TimingMiddleware
    {
        private readonly ILogger<TimingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public TimingMiddleware(ILogger<TimingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var start = DateTime.UtcNow;
            await _next.Invoke(httpContext);
            _logger.LogInformation($"Request {httpContext.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds} ms");
        }
    }


    public static class TimingMiddlewareExtensions
    {
        public static IApplicationBuilder UseTimingLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TimingMiddleware>();
        }
    }
}
