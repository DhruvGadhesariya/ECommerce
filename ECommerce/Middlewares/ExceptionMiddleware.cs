using System.Net;
using Newtonsoft.Json;

namespace ECommerce.Middlewares
{
    /// <summary>
    /// Global exception handling middleware.
    /// Logs unhandled exceptions and returns a standardized JSON response.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Log error with request details
                _logger.LogError(ex,
                    "Unhandled exception occurred while processing request {Method} {Path}",
                    httpContext.Request.Method, httpContext.Request.Path);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Converts exception into a JSON response with appropriate status code.
        /// </summary>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            // Customize certain known exceptions if required
            if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;
            else if (exception is ArgumentException) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = (int)code,
                Message = exception.Message, // For production, you may prefer: "An internal error occurred."
                Detail = exception.GetType().Name // Optional: helps debugging without exposing stack trace
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
