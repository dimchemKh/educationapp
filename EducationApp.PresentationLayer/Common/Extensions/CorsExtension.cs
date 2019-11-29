using EducationApp.BusinessLogic.Models.Configs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EducationApp.Presentation.Common.Extensions
{
    public static class CorsExtension
    {
        public static void AddCorsWithOrigin(this IServiceCollection services)
        {
            var corsService = services.BuildServiceProvider().GetService<IOptions<CorsConfig>>();

            services.AddCors(opt =>
            {
                opt.AddPolicy(corsService.Value.PolicyName, builder =>
                {
                    builder.WithOrigins(corsService.Value.Origins)
                           .AllowCredentials()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
    }
}
