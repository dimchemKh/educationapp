using EducationApp.BusinessLayer.Initializers;
using EducationApp.DataAccessLayer.Initializer;
using EducationApp.PresentationLayer.Common;
using EducationApp.PresentationLayer.Helper;
using EducationApp.PresentationLayer.Helper.Interfaces;
using EducationApp.PresentationLayer.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NLog;
using System.IO;
using EducationApp.BusinessLayer.Common.Extensions;
using EducationApp.PresentationLayer.Common.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace EducationApp.PresentationLayer
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

            JwtExtensions.AddJwt(services, Configuration);

            CorsExtension.AddCors(services, Configuration);

            InitializerServices.InitializeServices(services, Configuration);

            SwaggerExtensions.AddSwagger(services, Configuration);

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(Configuration.GetSection("Cors").GetSection("PolicyName").Value);

            SwaggerExtensions.UseSwagger(app, Configuration);

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
