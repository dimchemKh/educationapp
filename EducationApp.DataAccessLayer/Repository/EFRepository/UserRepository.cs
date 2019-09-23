using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class UserRepository : BaseEFRepository<BaseEntity>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserRepository(ApplicationContext context, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager, SignInManager<ApplicationUser> signInManager) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<ApplicationUser> SignUpAsync(string firstName, string lastName, string email, string password)
        {

            ApplicationUser user = new ApplicationUser() { FirstName = firstName, LastName = lastName, Email = email, UserName = firstName + lastName };

            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                await _signInManager.SignInAsync(user, isPersistent: true);
            }
            return user;
        }
        public async Task<string> GetEmailConfirmTokenAsync(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return code;
        }
        public Task SignInAsync(string email, string password)
        {
            return null;
        }

        public Task<ApplicationUser> GetUser(int userId) => throw new NotImplementedException();
        public Task<Role> GetUserRole(int userId) => throw new NotImplementedException();
        public Task ChangePassword(string email) => throw new NotImplementedException();
        public Task<bool> ConfirmEmail(bool confirm)
        {
            //_userManager.GenerateEmailConfirmationTokenAsync()
            return null;
        }
        public Task<ApplicationUser> UpdateUser(int userId) => throw new NotImplementedException();
        public Task<ApplicationUser> DeleteUser(int userId)
        {
            return null;
        }
    }
}
