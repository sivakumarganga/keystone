namespace KeyStone.API.Middlewares
{
    public class RequestIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestIdMiddleware> _logger;
        public RequestIdMiddleware(RequestDelegate next, ILogger<RequestIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;

            _logger.LogInformation("New Request Id: {RequestId} was generated", requestId);

            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append("X-Request-Id", requestId);
                return Task.CompletedTask;
            });
            await _next(context);
        }
    }
}
