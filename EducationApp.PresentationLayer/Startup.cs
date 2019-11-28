using EducationApp.DataAccessLayer.Initializers;
using EducationApp.Presentation.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EducationApp.Presentation.Common.Extensions;
using NLog;
using System.IO;
using PresentationInitializers = EducationApp.Presentation.Initializers;
using BusinessLogicInitializers = EducationApp.BusinessLogic.Initializers;
using DataAccessInitializers = EducationApp.DataAccessLayer.Initializers;

namespace EducationApp.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;                
            });

            PresentationInitializers.InitializerServices.Initialize(services, Configuration);

            BusinessLogicInitializers.InitializerServices.Initialize(services);

            DataAccessInitializers.InitializerServices.Initialize(services, Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbBaseInitializing dbBaseInitializing)
        {            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Base/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(Configuration.GetSection("Cors").GetSection("PolicyName").Value);

            app.UseSwagger(Configuration);

            app.UseMiddleware<ExceptionMiddleware>();

            dbBaseInitializing.Initialize();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
    

}
