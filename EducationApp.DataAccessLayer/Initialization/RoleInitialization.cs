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
        private static readonly string[] roles = { "Admin", "User" };

        public static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            foreach (var role in roles)
            {

                if (!await roleManager.RoleExistsAsync(role))
                {
                    var create = await roleManager.CreateAsync(new Role() { Name = role });

                    if (!create.Succeeded)
                    {
                        throw new Exception("Fail to create Roles");
                    }
                }
            }
        }
    }
}
