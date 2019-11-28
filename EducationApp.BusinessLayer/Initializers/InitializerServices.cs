using EducationApp.BusinessLogic.Common;
using EducationApp.BusinessLogic.Helpers;
using EducationApp.BusinessLogic.Helpers.Interfaces;
using EducationApp.BusinessLogic.Services;
using EducationApp.BusinessLogic.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using EducationApp.BusinessLogic.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLogic.Helpers.Mappers;
using EducationApp.BusinessLogic.Common.Interfaces;
using EducationApp.BusinessLogic.Common.Extensions;

namespace EducationApp.BusinessLogic.Initializers
{
    public static class InitializerServices
    {
        public static void Initialize(IServiceCollection services)
        {
            #region Services
            services.AddSingleton<ILoggerNLog, LoggerNLog>();

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
        }
    }
}
