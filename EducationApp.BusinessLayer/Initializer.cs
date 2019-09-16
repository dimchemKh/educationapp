using EducationApp.BusinessLayer.Common;
using EducationApp.BusinessLayer.Common.Interfaces;
using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Services;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Initialization;
using Microsoft.AspNetCore.Builder;
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
        public static void InitServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<ILoggerProvider, LoggerProvider>(sp => new LoggerProvider(filePath: Path.Combine(Directory.GetCurrentDirectory(), "logging.txt")));

            services.AddScoped(typeof(RoleInitialization));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 6;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Lockout.AllowedForNewUsers = true;
                
                options.User.RequireUniqueEmail = true;
            });


            services.AddTransient<EmailHelper>();
        }
    }
}
