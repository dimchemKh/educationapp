using EducationApp.BusinessLayer.Common;
using EducationApp.BusinessLayer.Common.Interfaces;
using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Services;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Initialization;
using EducationApp.DataAccessLayer.Repository.EFRepository;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer
{
    public static class Initializer
    {
        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void InitServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();


            #region Services

            services.AddSingleton<ILoggerProvider, LoggerProvider>(sp => new LoggerProvider(filePath: Path.Combine(Directory.GetCurrentDirectory(), "logging.txt")));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped(typeof(RoleInitialization));

            #endregion

            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();

            #endregion


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 6;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Lockout.AllowedForNewUsers = true;
                
                options.User.RequireUniqueEmail = true;
            });
            
            services.AddTransient<EmailHelper>();
        }

        public static void InitApp(IApplicationBuilder app, IHostingEnvironment env)
        {
            
        }
    }
}
