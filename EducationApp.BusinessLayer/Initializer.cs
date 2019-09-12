using EducationApp.BusinessLayer.Common;
using EducationApp.BusinessLayer.Common.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Initialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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

            //new DataAccessLayer.Initialization.DbBaseInitializing().Init();

            services.AddSingleton<ILogger, Logger>();
        }
        public static  void Init(IApplicationBuilder app, RoleManager<IdentityRole> roleManager)
        {
            RoleInitialization.SeedRoles(roleManager).Wait();
        }
        
    }
}
