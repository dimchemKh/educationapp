using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using EducationApp.DataAccessLayer.Entities.Enums;

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
        public async Task<bool> SignUpAsync(ApplicationUser user, string password)
        {            
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Constants.Roles.User);
                await _signInManager.SignInAsync(user, isPersistent: true);
            }            
            return result.Succeeded;
        }
        public async Task<IList<string>> GetRoleAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<string> GetEmailConfirmTokenAsync(ApplicationUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
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
            return user;
        }
        public async Task<IEnumerable<IdentityError>> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Errors;
        }
        public async Task<string> GenerateResetPasswordTokenAsync(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<bool> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
        public async Task<IEnumerable<ApplicationUser>> GetFilteredDataAsync(FilterUserModel model)
        {
            IQueryable<ApplicationUser> listUsers = null;

            if (string.IsNullOrWhiteSpace(model.SearchString))
            {
                listUsers = _userManager.Users.Where(user => user.IsRemoved.Equals(false));
            }
            if (!string.IsNullOrWhiteSpace(model.SearchString))
            {
                listUsers = _userManager.Users.Where(user => user.IsRemoved.Equals(false)&& user.FirstName.Contains(model.SearchString)
                || user.LastName.Contains(model.SearchString));
            }

            Expression<Func<ApplicationUser, object>> lambda = null;
            if (model.SortType.Equals(Enums.SortType.Name))
            {
                lambda = x => x.UserName;
            }
            if (model.SortType.Equals(Enums.SortType.Email))
            {
                lambda = x => x.Email;
            }

            if (model.Blocked.Equals(Enums.IsBlocked.True))
            {
                listUsers = listUsers.Where(x => x.LockoutEnabled.Equals(true));
            }
            if (model.Blocked.Equals(Enums.IsBlocked.False))
            {
                listUsers = listUsers.Where(x => x.LockoutEnabled.Equals(false));
            }

            if (model.SortState.Equals(Enums.SortState.Asc))
            {
                listUsers = listUsers.OrderBy(lambda);
            }
            if (model.SortState.Equals(Enums.SortState.Desc))
            {
                listUsers = listUsers.OrderByDescending(lambda);
            }
 
            return await listUsers.Skip(model.Page - 1 * model.PageSize).Take(model.PageSize).ToListAsync();
        }
    }
}
