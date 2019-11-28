using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.EFRepository;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using dapper = EducationApp.DataAccessLayer.Repository.DapperRepositories;

namespace EducationApp.DataAccessLayer.Initializers
{
    public static class InitializerServices
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DbBaseInitializing>();

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, Role>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            #region Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<IPrintingEditionRepository, PrintingEditionRepository>();
            //services.AddTransient<IAuthorInPrintingEditionRepository, AuthorInPrintingEditionRepository>();
            //services.AddTransient<IAuthorRepository, AuthorRepository>();
            //services.AddTransient<IOrderRepository, OrderRepository>();
            //services.AddTransient<IPaymentRepository, PaymentRepository>();
            #endregion

            #region
            services.AddTransient<IPrintingEditionRepository, dapper.PrintingEditionRepository>();
            services.AddTransient<IAuthorInPrintingEditionRepository, dapper.AuthorInPrintingEditionRepository>();
            services.AddTransient<IAuthorRepository, dapper.AuthorRepository>();
            services.AddTransient<IOrderRepository, dapper.OrderRepository>();
            services.AddTransient<IOrderItemRepository, dapper.OrderItemRepository>();
            services.AddTransient<IPaymentRepository, dapper.PaymentRepository>();

            #endregion

        }
    }
}
