using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace EducationApp.DataAccessLayer.Initializer
{
    public class DbBaseInitializing
    {
        protected readonly ApplicationContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<Role> _roleManager;

        public DbBaseInitializing(ApplicationContext context, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async void Initialize()
        {
            if(!await _roleManager.Roles.AnyAsync(role => role.Name == "admin"))
            {
                SeedAdminRoles();
            }

            _context.Authors.Add(new Author()
            {
                Name = "TestAuthor",
                CreationDate = DateTime.Now
            });
            _context.PrintingEditions.Add(new PrintingEdition()
            {
                Name = "TestBook",
                Description = "Some description",
                Price = 100,
                Status = Entities.Enums.Enums.Status.None,
                CreationDate = DateTime.Now,
                Type = Entities.Enums.Enums.Type.Book,
                Currency = Entities.Enums.Enums.Currency.USD
            });

        }
        private async void SeedAdminRoles()
        {
            string adminEmail = "admin@gmail.com";
            string password = "QWEqwe123qwe";

            await _roleManager.CreateAsync(new Role() { Name = Constants.Roles.Admin });
            await _roleManager.CreateAsync(new Role() { Name = Constants.Roles.User });

            var admin = new ApplicationUser { FirstName = "Main", LastName = "Admin", Email = adminEmail, UserName = adminEmail };

            var result = await _userManager.CreateAsync(admin, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, Constants.Roles.Admin);
            }
        }
    }
}
