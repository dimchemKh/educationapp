using EducationApp.PresentationLayer.Common;
using EducationApp.PresentationLayer.Helper.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Helper
{
    public static class JwtHelper /*: IJwtHelper*/
    {
        public static void GenerateJwt(IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("JwtConfig");

            services.Configure<Config>(jwtConfig);

            var appSettings = jwtConfig.Get<Config>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.JwtKey));

            var tokenValidationParametr = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParametr;
            });
        }

        public static string GenerateToken(List<Claim> claims, IOptions<Config> configOptions, TimeSpan tokenExpiration)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configOptions.Value.JwtKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            var token = new JwtSecurityToken(
             issuer: configOptions.Value.JwtIssuer,
             audience: "WebApiEducationApp",
             claims: claims,
             expires: new DateTime().Add(tokenExpiration),
             signingCredentials: credential);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
