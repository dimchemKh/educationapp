using EducationApp.PresentationLayer.Helper;
using EducationApp.PresentationLayer.Helper.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Common.Extensions
{
    public static class JwtExtensions
    {
        public static void AddJwt(IServiceCollection services, IConfiguration configuration)
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

            services.AddScoped<IJwtHelper, JwtHelper>();
        }
    }
}
