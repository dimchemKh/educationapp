using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Initialization
{
    public static class RoleInitialization
    {
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
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "qwerty";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new Role() { Name = "admin" });
            }

            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new Role() { Name = "user" });
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail };

                IdentityResult result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
