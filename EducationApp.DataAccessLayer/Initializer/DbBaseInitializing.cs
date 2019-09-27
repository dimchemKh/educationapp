using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void Initialize()
        {
            if(!_roleManager.Roles.Any(role => role.Name == Constants.Roles.Admin || role.Name == Constants.Roles.User))
            {
                SeedAdminAndUserRoles();
            }
            if (!_context.Authors.Any())
            {
                SeedEntitiesAsync();
            }
        }
        protected void SeedAdminAndUserRoles()
        {
            string adminEmail = "admin@gmail.com";
            string password = "QWEqwe123qwe";

            _roleManager.CreateAsync(new Role() { Name = Constants.Roles.Admin }).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new Role() { Name = Constants.Roles.User }).GetAwaiter().GetResult();

            var admin = new ApplicationUser { FirstName = "Main", LastName = "Admin", Email = adminEmail, UserName = adminEmail };

            var result = _userManager.CreateAsync(admin, password).GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(admin, Constants.Roles.Admin).GetAwaiter().GetResult();
            }
            
        }
        protected void SeedEntitiesAsync()
        {
            _context.Authors.Add(new Author()
            {
                Name = "Bob Mart",
                CreationDate = DateTime.Now
            });
            _context.Authors.Add(new Author()
            {
                Name = "Avreliy Berk",
                CreationDate = DateTime.Now
            });
            _context.PrintingEditions.Add(new PrintingEdition()
            {
                Name = "Avto buisness",
                Description = "Some description",
                Price = 100,
                CreationDate = DateTime.Now,
                Type = Enums.Type.Book,
                Currency = Enums.Currency.USD

            });
            _context.PrintingEditions.Add(new PrintingEdition()
            {
                Name = "Concepts",
                Description = "Some description",
                Price = 80,
                CreationDate = DateTime.Now,
                Type = Enums.Type.Journal,
                Currency = Enums.Currency.USD
            });
            _context.SaveChanges();
        }
    }
}
