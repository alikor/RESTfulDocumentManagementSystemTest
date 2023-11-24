using System.Diagnostics;

namespace Documents.Middleware
{

    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiLoggingMiddleware> _logger;

        public ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            var elapsedTime = stopwatch.Elapsed;
            _logger.LogInformation($"Request to {context.Request.Path} took {elapsedTime.TotalMilliseconds} ms");
        }
    }
}