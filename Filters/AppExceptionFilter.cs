using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PennyPal.Exceptions;

namespace PennyPal.Filters
{
    public class AppExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<AppExceptionFilter> _logger;

        public AppExceptionFilter(ILogger<AppExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;

            if (ex is NotFoundException notFoundEx)
            {
                _logger.LogWarning(ex, "Not Found");
                context.Result = new NotFoundObjectResult(new
                {
                    StatusCode = 404,
                    Message = notFoundEx.Message
                });


            }

            else if (ex is CustomValidationException validationEx)
            {
                _logger.LogWarning(ex, "Validation error");
                context.Result = new BadRequestObjectResult(new
                {
                    StatusCode = 400,
                    Message = validationEx.Message
                });

            }
            else if (ex is Unauthorized unauthorizedEx)
            {
                _logger.LogWarning(ex, "Unauthorized access");
                context.Result = new ObjectResult(new
                {
                    StatusCode = unauthorizedEx.StatusCode,
                    Message = unauthorizedEx.Message
                })
                {
                    StatusCode = unauthorizedEx.StatusCode
                };
            }
            else
            {
                _logger.LogError(ex, "Unhandled exception");
                return Task.CompletedTask;
            }
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}