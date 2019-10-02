using EducationApp.BusinessLayer.Common;
using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Services;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Initializer;
using EducationApp.DataAccessLayer.Repository.EFRepository;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using EducationApp.DataAccessLayer.Common.Constants;

namespace EducationApp.BusinessLayer.Initializers
{
    public static class InitializerServices
    {
        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void InitializeServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            #region Services
            services.AddSingleton<ILoggerProvider, LoggerProvider>(sp => new LoggerProvider(Path.Combine(Directory.GetCurrentDirectory(), "logging.txt")));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPrintingEditionService, PrintingEditionService>();

            services.AddScoped<DbBaseInitializing>();

            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<IPasswordHelper, PasswordHelper>();
            services.AddScoped<IConverterHelper, ConverterHelper>();
            #endregion

            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPrintingEditionRepository, PrintingEditionRepository>();
            services.AddScoped<IAuthorInPrintingEditionRepository, AuthorInPrintingEditionRepository>();

            #endregion

            #region IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = Constants.PasswordsOptions.RequiredLength;
                options.Password.RequiredUniqueChars = Constants.PasswordsOptions.RequiredUniqueChars;
                options.Password.RequireDigit = Constants.PasswordsOptions.RequireDigit;
                options.Password.RequireUppercase = Constants.PasswordsOptions.RequireUppercase;
                options.Password.RequireLowercase = Constants.PasswordsOptions.RequireLowercase;
                options.Password.RequireNonAlphanumeric = Constants.PasswordsOptions.RequireNonAlphanumeric;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Lockout.AllowedForNewUsers = true;
                
                options.User.RequireUniqueEmail = true;
            });
            #endregion
        }
    }
}
