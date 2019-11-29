using EducationApp.BusinessLogic.Models.Configs;
using EducationApp.Presentation.Common.Extensions;
using EducationApp.Presentation.Common.Models.Configs;
using EducationApp.Presentation.Helper;
using EducationApp.Presentation.Helper.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogicInitializers = EducationApp.BusinessLogic.Initializers;
using PasswordOptions = EducationApp.BusinessLogic.Models.Configs.PasswordOptions;

namespace EducationApp.Presentation.Initializers
{
    public static class InitializerServices
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SwaggerConfig>(configuration.GetSection("SwaggerConfig"));
            services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));
            services.Configure<EmailConfig>(configuration.GetSection("EmailConfig"));
            services.Configure<CorsConfig>(configuration.GetSection("Cors"));
            
            services.AddCorsWithOrigin();

            services.AddJwt(configuration);

            services.AddScoped<IJwtHelper, JwtHelper>();

            services.AddSwagger();

            BusinessLogicInitializers.InitializerServices.Initialize(services, configuration);
        }
    }
}
