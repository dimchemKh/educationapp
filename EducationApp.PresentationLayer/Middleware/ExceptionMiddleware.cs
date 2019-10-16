using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EducationApp.PresentationLayer.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILoggerProvider _loggerProvider;
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next, ILoggerProvider loggerProvider)
        {
            _next = next;
            _loggerProvider = loggerProvider;
            _logger = _loggerProvider.CreateLogger("MiddlewareLoger");            
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.LogInformation("SOME LOGINFO: ", context.Request.Body);
                await _next(context);
            }
            catch (Exception)
            {
                _logger.LogError("Error", context.Response.StatusCode);
            }
        }
    }
}
