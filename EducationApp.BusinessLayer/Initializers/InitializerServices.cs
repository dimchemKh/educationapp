using EducationApp.BusinessLogic.Common;
using EducationApp.BusinessLogic.Helpers;
using EducationApp.BusinessLogic.Helpers.Interfaces;
using EducationApp.BusinessLogic.Services;
using EducationApp.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EducationApp.BusinessLogic.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLogic.Helpers.Mappers;
using EducationApp.BusinessLogic.Common.Interfaces;
using DataAccessInitializers = EducationApp.DataAccessLayer.Initializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using PasswordOptions = EducationApp.BusinessLogic.Models.Configs.PasswordOptions;
using System;

namespace EducationApp.BusinessLogic.Initializers
{
    public static class InitializerServices
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {   
            var passwordOptions = services.BuildServiceProvider().GetService<IOptions<PasswordOptions>>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = passwordOptions.Value.RequiredLength;
                options.Password.RequiredUniqueChars = passwordOptions.Value.RequiredUniqueChars;
                options.Password.RequireDigit = passwordOptions.Value.RequireDigit;
                options.Password.RequireUppercase = passwordOptions.Value.RequireUppercase;
                options.Password.RequireLowercase = passwordOptions.Value.RequireLowercase;
                options.Password.RequireNonAlphanumeric = passwordOptions.Value.RequireNonAlphanumeric;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(passwordOptions.Value.DefaultLockoutTimeSpan);
                options.Lockout.MaxFailedAccessAttempts = passwordOptions.Value.MaxFailedAccessAttempts;
                options.Lockout.AllowedForNewUsers = passwordOptions.Value.AllowedForNewUsers;

                options.User.RequireUniqueEmail = passwordOptions.Value.RequireUniqueEmail;
            });

            services.AddSingleton<ILoggerNLog, LoggerNLog>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPrintingEditionService, PrintingEditionService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddTransient<IEmailHelper, EmailHelper>();
            services.AddTransient<IPasswordHelper, PasswordHelper>();
            services.AddTransient<ICurrencyConverterHelper, CurrencyConverterHelper>();
            services.AddTransient<IMapperHelper, MapperHelper>();
            
            DataAccessInitializers.InitializerServices.Initialize(services, configuration);
        }
    }
}
