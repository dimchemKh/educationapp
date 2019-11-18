using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using System.Threading.Tasks;
using EducationApp.DataAccessLayer.Repository.DapperRepositories.Interfaces;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
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
            if (!isAdmin)
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
        public async Task<GenericModel<ApplicationUser>> GetFilteredDataAsync(FilterUserModel filter)
        {
            var users = _userManager.Users.Where(user => user.IsRemoved == false);

            users = users.Where(user => user.Id != Constants.AdminSettings.AdminId);

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                users = _userManager.Users.Where(user => user.FirstName.Contains(filter.SearchString));
            }

            if (filter.IsBlocked.Equals(Enums.IsBlocked.True))
            {
                users = users.Where(x => x.LockoutEnd != null);
            }
            if (filter.IsBlocked.Equals(Enums.IsBlocked.False))
            {
                users = users.Where(x => x.LockoutEnd == null);
            }

            Expression<Func<ApplicationUser, object>> predicate = x => x.FirstName;

            if (filter.SortType.Equals(Enums.SortType.Email))
            {
                predicate = x => x.Email;
            }

            var responseModel = new GenericModel<ApplicationUser>()
            {
                CollectionCount = users.Count()
            };

            if (filter.SortState.Equals(Enums.SortState.Asc))
            {
                users = users.OrderBy(predicate);
            }
            if (filter.SortState.Equals(Enums.SortState.Desc))
            {
                users = users.OrderByDescending(predicate);
            }

            var usersPage = users.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToList();

            responseModel.Collection = usersPage;

            return responseModel;
        }

        public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task BlockUserAsync(ApplicationUser user, bool isBlocked)
        {
            if (!isBlocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(1));
            }
            if (isBlocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
        }
    }
}
