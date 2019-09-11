using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer
{
    public class BusinessMiddleware
    {
        private readonly RequestDelegate _next;
        private Common.Logger _logger;

        public BusinessMiddleware(RequestDelegate next, Common.Logger logger)
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
                await ExceptionFilter(context, ex);                
            }
        }
    }
}
