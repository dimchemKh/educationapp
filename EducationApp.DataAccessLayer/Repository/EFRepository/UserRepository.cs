using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using EducationApp.DataAccessLayer.Models.Filters;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> SignUpAsync(string firstName, string lastName, string email, string password)
        {
            var user = new ApplicationUser()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = firstName + lastName
            };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                await _signInManager.SignInAsync(user, isPersistent: true);
            }            
            return result.Succeeded;
        }
        public async Task<IList<string>> GetRoleAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<string> GetEmailConfirmTokenAsync(long userId)
        {
            var user = await GetUserByIdAsync(userId);
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user ?? null;
        }
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            return result.Succeeded;
        }
        public async Task<bool> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user ?? null;
        }
        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            if (currentPassword.Equals(newPassword))
            {
                return IdentityResult.Failed(new IdentityError { Description = Constants.Errors.SamePasswords });
            }
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
        public async Task<string> GenerateResetPasswordTokenAsync(long userId)
        {
            var user = await GetUserByIdAsync(userId);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<bool> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
        public async Task<IEnumerable<ApplicationUser>> Filtering(FilterUserModel model)
        {
            IQueryable<ApplicationUser> listUsers = null;

            if (string.IsNullOrWhiteSpace(model.SearchByBody))
            {
                listUsers = _userManager.Users.Where(user => user.IsRemoved == false);
            }
            if (!string.IsNullOrWhiteSpace(model.SearchByBody))
            {
                listUsers = _userManager.Users.Where(user => user.IsRemoved == false && user.FirstName.Contains(model.SearchByBody)
                || user.LastName.Contains(model.SearchByBody));
            }

            Expression<Func<ApplicationUser, object>> lambda = null;
            if (model.SortType == Entities.Enums.Enums.SortType.Name)
            {
                lambda = x => x.UserName;
            }
            if (model.SortType == Entities.Enums.Enums.SortType.Email)
            {
                lambda = x => x.Email;
            }

            if (model.Blocked.Count == 1 && model.Blocked.Contains(Entities.Enums.Enums.IsBlocked.True))
            {
                listUsers = listUsers.Where(x => x.LockoutEnabled == false);
            }
            if (model.Blocked.Count == 1 && model.Blocked.Contains(Entities.Enums.Enums.IsBlocked.False))
            {
                listUsers = listUsers.Where(x => x.LockoutEnabled == true);
            }                       

            if(model.SortState == Entities.Enums.Enums.SortState.Asc)
            {
                listUsers = listUsers.OrderBy(lambda);
            }
            if (model.SortState == Entities.Enums.Enums.SortState.Desc)
            {
                listUsers = listUsers.OrderByDescending(lambda);
            }
 
            return await listUsers.Skip(model.Page - 1 * model.PageSize).Take(model.PageSize).ToListAsync();
        }
    }
}
