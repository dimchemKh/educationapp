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
using EducationApp.DataAccessLayer.Models;

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
        public async Task<IEnumerable<IdentityError>> SignUpAsync(ApplicationUser user, string password)
        {            
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Constants.Roles.User);
                await _signInManager.SignInAsync(user, isPersistent: true);
            }            
            return result.Errors;
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
        public async Task<SignInResult> CheckPasswordAsync(ApplicationUser user, string password, bool isAdmin = false)
        {
            if(!isAdmin)
            {
                return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            }
            return SignInResult.Success;
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
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }
        public async Task<bool> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
        public async Task<GenericModel<ApplicationUser>> GetFilteredDataAsync(FilterUserModel model)
        {
            IQueryable<ApplicationUser> listUsers = null;

            if (string.IsNullOrWhiteSpace(model.SearchString))
            {
                listUsers = _userManager.Users.Where(user => user.IsRemoved == false);
            }

            if (!string.IsNullOrWhiteSpace(model.SearchString))
            {
                listUsers = _userManager.Users.Where(user => user.IsRemoved == false && user.FirstName.Contains(model.SearchString)
                || user.LastName.Contains(model.SearchString));
            }

            listUsers = listUsers.Where(user => user.Id != Constants.AdminSettings.AdminId);

            Expression<Func<ApplicationUser, object>> lambda = x => x.FirstName;

            if (model.SortType.Equals(Enums.SortType.Email))
            {
                lambda = x => x.Email;
            }

            if (model.IsBlocked.Equals(Enums.IsBlocked.True))
            {
                listUsers = listUsers.Where(x => x.LockoutEnd != null);
            }
            if (model.IsBlocked.Equals(Enums.IsBlocked.False))
            {
                listUsers = listUsers.Where(x => x.LockoutEnd == null);
            }

            if (model.SortState.Equals(Enums.SortState.Asc))
            {
                listUsers = listUsers.OrderBy(lambda);
            }
            if (model.SortState.Equals(Enums.SortState.Desc))
            {
                listUsers = listUsers.OrderByDescending(lambda);
            }

            var list = await listUsers.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToArrayAsync();

            var responseModel = new GenericModel<ApplicationUser>()
            {
                //Collection = list,
                //CollectionCount = await listUsers.CountAsync()
            };

            return responseModel;
        }

        public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task BlockUserAsync(ApplicationUser user, bool isBlocked)
        {
            if (isBlocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(1));
            }
            if(!isBlocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
        }
    }
}
