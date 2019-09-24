using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> SignUpAsync(string firstName, string lastName, string email, string password);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<string> GetEmailConfirmTokenAsync(ApplicationUser user);
        Task<ApplicationUser> SignInAsync(string email, string password);
        Task<string> GetRoleAsync(ApplicationUser user);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user);
        Task<string> GetRoleAsync(string userId);
        Task SendRegistrationMailAsync(ApplicationUser user, string callbackUrl);
        Task<string> GetUserNameAsync(string userId);
    }
}
