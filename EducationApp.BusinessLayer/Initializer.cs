using EducationApp.BusinessLayer.Common;
using EducationApp.BusinessLayer.Common.Interfaces;
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
using System;
using System.Collections.Generic;
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

            //new DataAccessLayer.Initialization.DbBaseInitializing().Init();

            services.AddSingleton<ILogger, Logger>();

            //services.AddScoped<IUserService, UserService>();

            services.AddSingleton(typeof(RoleInitialization));

            //services.AddScoped(IPrintingEditionService, PrintingEditionService);
        }

        //public static  void Init(IApplicationBuilder app, RoleManager<Role> roleManager)
        //{
        //    RoleInitialization.SeedRoles(roleManager).Wait();
        //}

        //public static async Task Init(IServiceScope scope)
        //{
        //    var services = scope.ServiceProvider;

        //    try
        //    {
        //        var user = services.GetRequiredService<UserManager<ApplicationUser>>();
        //        var role = services.GetRequiredService<RoleManager<Role>>();
        //        await RoleInitialization.InitializeAsync(user, role);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
