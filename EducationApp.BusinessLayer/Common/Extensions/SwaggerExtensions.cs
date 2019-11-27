using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace EducationApp.BusinessLayer.Common.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwagger(IServiceCollection services, IConfiguration configuration)
        {
            var title = configuration.GetSection("SwaggerConfig").GetSection("Title").Value;
            var version = configuration.GetSection("SwaggerConfig").GetSection("Version").Value;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new Info { Title = title, Version = version });
            });
        }
        public static void UseSwagger(IApplicationBuilder app, IConfiguration configuration)
        {
            var title = configuration.GetSection("SwaggerConfig").GetSection("Title").Value;
            var version = configuration.GetSection("SwaggerConfig").GetSection("Version").Value;
            var path = configuration.GetSection("SwaggerConfig").GetSection("Path").Value;

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(path, $"{title} {version}");
            });
        }
    }
}
