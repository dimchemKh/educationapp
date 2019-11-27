using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Common.Extensions
{
    public static class CorsExtension
    {
        public static void AddCors(IServiceCollection services, IConfiguration configuration)
        {
            var origin = configuration.GetSection("Cors").GetSection("Origins").Value;
            var policyName = configuration.GetSection("Cors").GetSection("PolicyName").Value;

            services.AddCors(opt =>
            {
                opt.AddPolicy(policyName, builder =>
                {
                    builder.WithOrigins(origin)
                           .AllowCredentials()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
    }
}
