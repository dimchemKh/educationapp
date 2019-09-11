using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var message = "Error";

            var exceptionType = context.Exception.GetType();
            if (exceptionType is ExceptionFilter) // CustomException
            {
                message = context.Exception.Message;
            }

            context.ExceptionHandled = true;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";
            context.Result = new ObjectResult(null);
        }
    }
}
