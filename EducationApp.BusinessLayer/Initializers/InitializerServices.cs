using EducationApp.BusinessLayer.Common;
using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Services;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Initializer;

using EducationApp.DataAccessLayer.Repository.Interfaces;

using EF = EducationApp.DataAccessLayer.Repository.EFRepository;
using dapper = EducationApp.DataAccessLayer.Repository.DapperRepositories;

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
            //services.AddTransient<IUserRepository, EF.UserRepository>();
            //services.AddTransient<IPrintingEditionRepository, EF.PrintingEditionRepository>();
            //services.AddTransient<IAuthorInPrintingEditionRepository, EF.AuthorInPrintingEditionRepository>();
            //services.AddTransient<IAuthorRepository, EF.AuthorRepository>();
            //services.AddTransient<IOrderRepository, EF.OrderRepository>();
            //services.AddTransient<IPaymentRepository, EF.PaymentRepository>();
            #endregion

            #region
            services.AddTransient<IUserRepository, dapper.UserRepository>();
            services.AddTransient<IPrintingEditionRepository, dapper.PrintingEditionRepository>();
            services.AddTransient<IAuthorInPrintingEditionRepository, dapper.AuthorInPrintingEditionRepository>();
            services.AddTransient<IAuthorRepository, dapper.AuthorRepository>();
            services.AddTransient<IOrderRepository, dapper.OrderRepository>();
            services.AddTransient<IOrderItemRepository, dapper.OrderItemRepository>();
            services.AddTransient<IPaymentRepository, dapper.PaymentRepository>();

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
