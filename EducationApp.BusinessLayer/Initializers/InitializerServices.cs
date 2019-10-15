using EducationApp.BusinessLayer.Common;
using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Services;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Initializer;
using EducationApp.DataAccessLayer.Repository.EFRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;

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

            services.AddScoped<DbBaseInitializing>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPrintingEditionService, PrintingEditionService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IOrderService, OrderService>();


            services.AddTransient<IEmailHelper, EmailHelper>();
            services.AddTransient<IPasswordHelper, PasswordHelper>();
            services.AddTransient<IConverterHelper, ConverterHelper>();
            services.AddTransient<IMapperHelper, MapperHelper>();

            #endregion

            #region Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPrintingEditionRepository, PrintingEditionRepository>();
            services.AddTransient<IAuthorInPrintingEditionRepository, AuthorInPrintingEditionRepository>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
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
