using EducationApp.BusinessLogic.Models.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace EducationApp.Presentation.Common.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            var swaggerService = services.BuildServiceProvider().GetService<IOptions<SwaggerConfig>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerService.Value.Version, new Info { Title = swaggerService.Value.Title, Version = swaggerService.Value.Version });
            });
        }
        public static void UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerSection = configuration.GetSection("SwaggerConfig");

            var title = swaggerSection.GetSection("Title").Value;
            var version = swaggerSection.GetSection("Version").Value;
            var path = swaggerSection.GetSection("Path").Value;

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(path, $"{title} {version}");
            });
        }
    }
}
