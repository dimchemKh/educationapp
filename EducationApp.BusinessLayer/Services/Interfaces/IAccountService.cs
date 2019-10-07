﻿using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> SignUpAsync(UserRegistrationModel userModel);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<string> GetEmailConfirmTokenAsync(ApplicationUser user);
        Task<ApplicationUser> SignInAsync(UserLoginModel loginModel);
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<bool> ResetPasswordAsync(ApplicationUser user);
        Task<string> GetRoleAsync(string userId);
        Task SendRegistrationMailAsync(ApplicationUser user, string callbackUrl);
        Task<string> GetUserNameAsync(string userId);
    }
}
