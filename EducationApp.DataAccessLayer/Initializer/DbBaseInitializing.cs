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
            var pe1 = new PrintingEdition()
            {
                Name = "Avto buisness",
                Description = "Some description",
                Price = 100,
                CreationDate = DateTime.Now,
                Type = Enums.Type.Book,
                Currency = Enums.Currency.USD
            };
            var pe2 = new PrintingEdition()
            {
                Name = "New business",
                Description = "Some description",
                Price = 20,
                CreationDate = DateTime.Now,
                Type = Enums.Type.Newspaper,
                Currency = Enums.Currency.USD
            };
            var pe3 = new PrintingEdition()
            {
                Name = "Concepts",
                Description = "Some description",
                Price = 80,
                CreationDate = DateTime.Now,
                Type = Enums.Type.Journal,
                Currency = Enums.Currency.USD
            };

            var author1 = new Author()
            {
                Name = "Bob Mart",
                CreationDate = DateTime.Now
            };
            var author2 = new Author()
            {
                Name = "Avreliy Berk",
                CreationDate = DateTime.Now
            };
            var author3 = new Author()
            {
                Name = "Edward Berilog",
                CreationDate = DateTime.Now
            };

            pe1.AuthorInBooks = new List<AuthorInBooks>()
            {
                new AuthorInBooks {
                    Author = author1,
                    PrintingEditionId = pe1.Id
                },
                new AuthorInBooks
                {
                    Author = author2,
                    PrintingEditionId = pe1.Id
                }
            };
            pe2.AuthorInBooks = new List<AuthorInBooks>()
            {
                new AuthorInBooks
                {
                    Author = author2,
                    PrintingEditionId = pe2.Id
                },
                new AuthorInBooks
                {
                    Author = author3,
                    PrintingEditionId = pe2.Id
                }
            };
            pe3.AuthorInBooks = new List<AuthorInBooks>()
            {
                new AuthorInBooks
                {
                    Author = author2,
                    PrintingEditionId = pe3.Id
                }
            };

            _context.Authors.AddRange(new List<Author>() { author1, author2, author3 });

            _context.PrintingEditions.AddRange(new List<PrintingEdition>() { pe1, pe2, pe3 });

            _context.SaveChanges();
        }
    }
}
