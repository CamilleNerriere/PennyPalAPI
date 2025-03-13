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
                context.Result = new ObjectResult(new
                {
                    StatusCode = 404,
                    Message = notFoundEx.Message
                })
                {
                    StatusCode = 404
                };

                _logger.LogWarning(ex, "Not Found");

            }

            else if (ex is CustomValidationException validationEx)
            {
                context.Result = new ObjectResult(new
                {
                    StatusCode = 400,
                    Message = validationEx.Message
                })
                {
                    StatusCode = 400
                };
                _logger.LogWarning(ex, "Validation error");

            }
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}