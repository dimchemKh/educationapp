using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using EducationApp.BusinessLogic.Common.Interfaces;

namespace EducationApp.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILoggerNLog _logger;
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next, ILoggerNLog logger)
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
                _logger.Error($"Status Code: {context.Response.StatusCode.ToString()} => Exception message: {ex.Message}");               
            }
        }
    }
}
