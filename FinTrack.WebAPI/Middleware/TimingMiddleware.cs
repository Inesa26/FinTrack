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

            try
            {
                await _next.Invoke(httpContext);
            }
            finally
            {
                var elapsedMilliseconds = (DateTime.UtcNow - start).TotalMilliseconds;
                var requestPath = httpContext.Request.Path;

                _logger.LogInformation("Request '{RequestPath}' processed in {ElapsedMilliseconds} ms", requestPath, elapsedMilliseconds);
            }
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
