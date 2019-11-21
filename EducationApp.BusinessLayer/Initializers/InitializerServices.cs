using EducationApp.BusinessLayer.Common;
using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Services;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Initializer;

using EducationApp.DataAccessLayer.Repository.EFRepository;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;

//using EducationApp.DataAccessLayer.Repository.DapperRepositories;
//using EducationApp.DataAccessLayer.Repository.DapperRepositories.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using EducationApp.BusinessLayer.Common.Constants;

using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLayer.Helpers.Mappers;
using EducationApp.BusinessLayer.Common.Interfaces;

namespace EducationApp.BusinessLayer.Initializers
{
    public static class InitializerServices
    {
        public static void InitializeServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            #region Services
            services.AddSingleton<ILog, LoggerNLog>();

            services.AddScoped<DbBaseInitializing>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPrintingEditionService, PrintingEditionService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IOrderService, OrderService>();


            services.AddTransient<IEmailHelper, EmailHelper>();
            services.AddTransient<IPasswordHelper, PasswordHelper>();
            services.AddTransient<ICurrencyConverterHelper, CurrencyConverterHelper>();
            services.AddTransient<IMapperHelper, MapperHelper>();

            #endregion

            #region Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPrintingEditionRepository, PrintingEditionRepository>();
            services.AddTransient<IAuthorInPrintingEditionRepository, AuthorInPrintingEditionRepository>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            //services.AddTransient<IOrderRepository, OrderRepository>();
            //services.AddTransient<IPaymentRepository, PaymentRepository>();
            #endregion
             
            #region IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = Constants.PasswordOptions.RequiredLength;
                options.Password.RequiredUniqueChars = Constants.PasswordOptions.RequiredUniqueChars;
                options.Password.RequireDigit = Constants.PasswordOptions.RequireDigit;
                options.Password.RequireUppercase = Constants.PasswordOptions.RequireUppercase;
                options.Password.RequireLowercase = Constants.PasswordOptions.RequireLowercase;
                options.Password.RequireNonAlphanumeric = Constants.PasswordOptions.RequireNonAlphanumeric;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Lockout.AllowedForNewUsers = true;
                
                options.User.RequireUniqueEmail = true;
            });
            #endregion
        }
    }
}
