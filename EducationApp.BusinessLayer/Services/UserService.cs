using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Services
{
    public class UserService : Interfaces.IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<Role> _signInManager;

        public UserService()
        {

        }
    }
}
