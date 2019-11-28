using EducationApp.Presentation.Common.Extensions;
using EducationApp.Presentation.Helper;
using EducationApp.Presentation.Helper.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EducationApp.Presentation.Initializers
{
    public static class InitializerServices
    {
        private static bool ParseBoolData(IConfigurationSection section, string sectionName)
        {
            if (bool.TryParse(section.GetSection(sectionName).Value, out bool _result))
            {
                return _result;
            }
            return false;
        }
        private static int ParseIntData(IConfigurationSection section, string sectionName)
        {
            if (int.TryParse(section.GetSection(sectionName).Value, out int _result))
            {
                return _result;
            }
            return 0;
        }
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            var passwordSection = configuration.GetSection("PasswordOptions");

            services.AddCors(configuration);

            services.AddJwt(configuration);

            services.AddScoped<IJwtHelper, JwtHelper>();

            services.AddSwagger(configuration);

            #region IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = ParseIntData(passwordSection, "RequiredLength");
                options.Password.RequiredUniqueChars = ParseIntData(passwordSection, "RequiredUniqueChars");
                options.Password.RequireDigit = ParseBoolData(passwordSection, "RequireDigit");
                options.Password.RequireUppercase = ParseBoolData(passwordSection, "RequireUppercase");
                options.Password.RequireLowercase = ParseBoolData(passwordSection, "RequireLowercase");
                options.Password.RequireNonAlphanumeric = ParseBoolData(passwordSection, "RequireNonAlphanumeric");

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(ParseIntData(passwordSection, "DefaultLockoutTimeSpan"));
                options.Lockout.MaxFailedAccessAttempts = ParseIntData(passwordSection, "MaxFailedAccessAttempts");
                options.Lockout.AllowedForNewUsers = ParseBoolData(passwordSection, "AllowedForNewUsers");

                options.User.RequireUniqueEmail = ParseBoolData(passwordSection, "RequireUniqueEmail");
            });
            #endregion
        }
    }
}
