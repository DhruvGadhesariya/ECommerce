using Newtonsoft.Json;
using System.Net;

namespace ECommerce.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
            // Customize the response based on the exception
            // For example, you can set the response status code and return an error message
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new
            {
                ErrorMessage = exception.Message,
                // Additional properties like stack trace, inner exceptions, etc.
            };

            // Serialize the error response to JSON
            var responseJson = JsonConvert.SerializeObject(errorResponse);

            return context.Response.WriteAsync(responseJson);

        }
    }
}