using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using EducationApp.BusinessLayer.Common.Interfaces;

namespace EducationApp.PresentationLayer.Middleware
{
    public class ExceptionMiddleware
    {
        private ILog _logger;
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next, ILog logger)
        {
            _next = next;
            _logger = logger;             
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.Information($"Status Code: {context.Response.StatusCode.ToString()}");
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error($"Status Code: {context.Response.StatusCode.ToString()} => Exception message: {ex.Message}");               
            }
        }
    }
}
