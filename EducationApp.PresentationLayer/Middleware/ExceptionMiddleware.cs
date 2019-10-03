using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using EducationApp.BusinessLayer.Common.Interfaces;
using System.Linq;
using EducationApp.BusinessLayer.Common;
using Microsoft.Extensions.Logging;

namespace EducationApp.PresentationLayer.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILoggerProvider _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILoggerProvider logger)
        {
            _next = next;
            _logger = logger;            
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.CreateLogger("CustomLOGGER").LogInformation("SOME LOGINFO: ", context.Response.StatusCode.ToString());
                await _next(context);
            }
            catch (Exception)
            {
                _logger.CreateLogger("Exception").LogError("Error", context.Response.StatusCode.ToString());
            }
        }
    }
}
