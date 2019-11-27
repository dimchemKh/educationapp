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
using Microsoft.OpenApi.Models;
using System.Text;
using NLog;
using System.IO;

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

            var jwtConfig = Configuration.GetSection("JwtConfig");
            services.Configure<AuthConfig>(jwtConfig);

            var appSettings = jwtConfig.Get<AuthConfig>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.JwtKey));

            var tokenValidationParametr = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                ValidIssuer = appSettings.JwtIssuer,
                ValidateIssuer = true,

                ValidAudience = appSettings.JwtAudience,
                ValidateAudience = true,

                ValidateLifetime = true                
            };

            services.AddAuthentication(scheme =>
            {
                scheme.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                scheme.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParametr;
            });

            services.AddScoped<IJwtHelper, JwtHelper>();


            InitializerServices.InitializeServices(services, Configuration);

            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Education Store", Version = "v1" });
            });
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Education Store V1");
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(builder => builder.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200"));

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
