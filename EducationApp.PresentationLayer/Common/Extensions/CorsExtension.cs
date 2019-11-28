using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EducationApp.Presentation.Common.Extensions
{
    public static class CorsExtension
    {
        public static void AddCors(this IServiceCollection services, IConfiguration configuration)
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
