using EducationApp.BusinessLayer;
using EducationApp.BusinessLayer.Common;
using EducationApp.PresentationLayer.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace EducationApp.PresentationLayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;                
            });

            Initializer.InitServices(services, Configuration);



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddFile(filePath: Path.Combine(Directory.GetCurrentDirectory(), "logging.txt"));
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDirectoryBrowser();
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
            //app.UseCookiePolicy();
            
            //app.UseStatusCodePages();

            app.UseAuthentication();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMvc();
        }
    }
}
