using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            _roleManager.CreateAsync(new Role()
            {
                Name = Constants.Roles.Admin
            }).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new Role()
            {
                Name = Constants.Roles.User
            }).GetAwaiter().GetResult();

            var admin = new ApplicationUser
            {
                FirstName = Constants.AdminSettings.FirstName,
                LastName = Constants.AdminSettings.LastName,
                Email = Constants.AdminSettings.Email,
                UserName = string.Concat(Constants.AdminSettings.FirstName, Constants.AdminSettings.LastName),
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            var result = _userManager.CreateAsync(admin, Constants.AdminSettings.Password).GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(admin, Constants.Roles.Admin).GetAwaiter().GetResult();
            }
            
        }
        protected void SeedEntitiesAsync()
        {
            var printingEdition1 = new PrintingEdition()
            {
                Title = "Avto buisness",
                Description = "Some description",
                Price = 100,
                CreationDate = DateTime.Now,
                PrintingEditionType = Enums.PrintingEditionType.Book,
                Currency = Enums.Currency.USD
            };
            var printingEdition2 = new PrintingEdition()
            {
                Title = "New business",
                Description = "Some description",
                Price = 20,
                CreationDate = DateTime.Now,
                PrintingEditionType = Enums.PrintingEditionType.Newspaper,
                Currency = Enums.Currency.USD
            };
            var printingEdition3 = new PrintingEdition()
            {
                Title = "Concepts",
                Description = "Some description",
                Price = 80,
                CreationDate = DateTime.Now,
                PrintingEditionType = Enums.PrintingEditionType.Journal,
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

            printingEdition1.AuthorInPrintingEditions = new List<AuthorInPrintingEdition>()
            {
                new AuthorInPrintingEdition
                {
                    Author = author1,
                    PrintingEdition = printingEdition1
                },
                new AuthorInPrintingEdition
                {
                    Author = author2,
                    PrintingEdition = printingEdition1
                }
            };
            printingEdition2.AuthorInPrintingEditions = new List<AuthorInPrintingEdition>()
            {
                new AuthorInPrintingEdition
                {
                    Author = author2,
                    PrintingEdition = printingEdition2
                },
                new AuthorInPrintingEdition
                {
                    Author = author3,
                    PrintingEdition = printingEdition2
                }
            };
            printingEdition3.AuthorInPrintingEditions = new List<AuthorInPrintingEdition>()
            {
                new AuthorInPrintingEdition
                {
                    Author = author2,
                    PrintingEdition = printingEdition3
                }
            };
            

            _context.Authors.AddRange(new List<Author>()
            {
                author1,
                author2,
                author3
            });

            _context.PrintingEditions.AddRange(new List<PrintingEdition>()
            {
                printingEdition1,
                printingEdition2,
                printingEdition3
            });

            _context.SaveChanges();
        }
    }
}
