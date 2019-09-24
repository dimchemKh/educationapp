using EducationApp.BusinessLayer.Helpers;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.EFRepository;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EducationApp.BusinessLayer.Helpers.Interfaces;

namespace EducationApp.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private IUserRepository _userRepository;
        private IGenerator _generator;
        private IEmailHelper _emailHelper;

        public AccountService(IUserRepository userRepository, IGenerator generator, IEmailHelper emailHelper)
        {
            _userRepository = userRepository;
            _generator = generator;
            _emailHelper = emailHelper;
        }
        public async Task<ApplicationUser> SignInAsync(string email, string password)
        {
            if(email == null || password == null)
            {
                return null;
            }
            var existedUser = await _userRepository.GetUserByEmailAsync(email);
            if(existedUser != null)
            {                
                if(await _userRepository.CheckPasswordAsync(existedUser, password))
                {
                    return existedUser;
                }
            }
            return null;                        
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
        public async Task<string> GetRoleAsync(ApplicationUser user)
        {
            string _role = null;
            var roles = await _userRepository.GetRoleAsync(user);
            foreach (var role in roles)
            {
                if (role.Contains("user"))
                {
                    _role = role;
                }
                else if (role.Contains("admin"))
                {
                    _role = role;
                }
            }
            return _role;
        }
        public async Task<bool> SignUpAsync(string firstName, string lastName, string email, string password)
        {
            var existedUser = await _userRepository.GetUserByEmailAsync(email);
            if(existedUser != null)
            {
                var result = await _userRepository.SignUpAsync(firstName, lastName, email, password);
                return result.Succeeded;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> GetEmailConfirmTokenAsync(ApplicationUser user)
        {
            return await _userRepository.GetEmailConfirmTokenAsync(user);
        }
        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if(user != null)
            {
                var result = await _userRepository.ConfirmEmail(user, token);                
                return result.Succeeded;
            }
            else
            {
                return false;
            }
        }

        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            return null;
        }
        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user)
        {
            var token = await _userRepository.GenerateResetPasswordTokenAsync(user);

            var newTempPassword = _generator.GenerateRandomPassword();

            string message = $"This is your Temp password {newTempPassword} ! Please change his, after succesfull authorization";
            string subject = "NewTempPassword";
            await _emailHelper.SendMailAsync(user, subject, message);

            return await _userRepository.ResetPasswordAsync(user, token, newTempPassword);
        }
        public async Task SendRegistrationMailAsync(ApplicationUser user, string callbackUrl)
        {

            var body = $"Hi, {user.FirstName}!" +
                $"You have been sent this email because you created an account on our website. " +
                $"Please click on <a href =\"" + callbackUrl + "\">this link</a> to confirm your email address is correct. ";

            await _emailHelper.SendMailAsync(user, "ConfirmEmail", body);
        }

        public async Task<string> GetRoleAsync(string userId)
        {
            string _role = null;
            var user = await _userRepository.GetUserByIdAsync(userId);
            var roles = await _userRepository.GetRoleAsync(user);
            foreach (var role in roles)
            {
                if (role.Contains("user"))
                {
                    _role = role;
                }
                else if (role.Contains("admin"))
                {
                    _role = role;
                }
            }
            return _role;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            
            return user?.UserName;
        }
    }
}
