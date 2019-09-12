using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Initialization
{
    public class RoleInitialization
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RoleInitialization(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        //private static readonly string[] roles = { "Admin", "User" };

        //public static async Task SeedRoles(RoleManager<Role> roleManager)
        //{
        //    foreach (var role in roles)
        //    {
        //        if (!await roleManager.RoleExistsAsync(role))
        //        {
        //            var create = await roleManager.CreateAsync(new Role() { Name = role });
        //            if (!create.Succeeded)
        //            {
        //                throw new Exception("Fail to create Roles");
        //            }
        //        }
        //    }
        //}
        public async void InitializeAsync(/*UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager*/)
        {
            string adminEmail = "admin@gmail.com";
            string password = "qwerty";

            if (await _roleManager.FindByNameAsync("admin") == null)
            {         
                await _roleManager.CreateAsync(new Role() { Name = "admin" });
            }         
                      
            if (await _roleManager.FindByNameAsync("user") == null)
            {         
                await _roleManager.CreateAsync(new Role() { Name = "user" });
            }         
                      
            if (await _userManager.FindByNameAsync(adminEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail };

                IdentityResult result = await _userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
