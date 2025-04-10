using System.Net;
using System.Text.Json;

namespace PennyPal.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                
                _logger.LogError(ex, "Unhandle Error");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new 
            {
                StatusCode = context.Response.StatusCode,
                Message = "An error occured. Please Try latter.",
                Details = ex.Message // NE PAS OUBLIER DE SUPPRIMER EN PROD !!!
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

