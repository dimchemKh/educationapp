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

namespace EducationApp.PresentationLayer.Middleware
{
    public class ExceptionMiddleware
    {
        private ILogger _logger;
        private readonly RequestDelegate _next;

        List<HttpStatusCode> codes = new List<HttpStatusCode>()
                {
                    HttpStatusCode.OK,
                    HttpStatusCode.NotFound,
                    HttpStatusCode.BadRequest,
                    HttpStatusCode.BadGateway,
                    HttpStatusCode.NotFound
                };

        public ExceptionMiddleware(RequestDelegate next, ILogger logger)
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
                var responseCode = context.Response?.StatusCode;
                                

                _logger.WriteMessage($"{context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())}" +
                                        $"{context.Request.Host} = { codes.Find(x => (int)x == responseCode)}");                
            }
        }
    }
}
