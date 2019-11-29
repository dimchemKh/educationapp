using EducationApp.Presentation.Common.Models.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace EducationApp.Presentation.Common.Extensions
{
    public static class JwtExtensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("JwtConfig");

            services.Configure<AuthConfig>(jwtConfig);

            var appSettings = jwtConfig.Get<AuthConfig>();

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.JwtKey));

            var tokenValidationParameter = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                ValidIssuer = appSettings.JwtIssuer,
                ValidateIssuer = true,

                ValidAudience = appSettings.JwtAudience,
                ValidateAudience = true,

                ValidateLifetime = true
            };

            services.AddAuthentication(scheme =>
            {
                scheme.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                scheme.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameter;
            });
        }
    }
}
