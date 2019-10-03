using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Filter
{
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        private readonly ILogger _logger;
        public ExceptionFilterAttribute(ILoggerProvider logger)
        {
            _logger = logger.CreateLogger("ExceptionCu8stomLog");
        }
        public void OnException(ExceptionContext context)
        {
            var name = context.ActionDescriptor.DisplayName;
            string exceptionMessage = context.Exception.Message;
            var headers = context.HttpContext.Response.StatusCode;

            _logger.LogError($"Name: {name}, exception {exceptionMessage} - headers = {headers.ToString()}");


        }
    }
}
