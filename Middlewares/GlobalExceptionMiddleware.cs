using System.Net;
using System.Text.Json;

namespace PennyPal.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Unhandle Error");
                await HandleExceptionAsync(context, ex, _env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, IWebHostEnvironment env)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new 
            {
                StatusCode = context.Response.StatusCode,
                Message = "An error occured. Please Try latter.",
                Details = env.IsDevelopment() ? ex.Message : "" 
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

