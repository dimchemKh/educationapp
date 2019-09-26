using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using System.Linq;
using EducationApp.BusinessLayer.Models.Users;

namespace EducationApp.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IEmailHelper _emailHelper;

        public AccountService(IUserRepository userRepository, IPasswordHelper passwordHelper, IEmailHelper emailHelper)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _emailHelper = emailHelper;
        }
        public async Task<ApplicationUser> SignInAsync(LoginModel loginModel)
        {
            if(loginModel == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(loginModel.Email) || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                return null;
            }
            var existedUser = await _userRepository.GetUserByEmailAsync(loginModel.Email);
            if (existedUser != null && await _userRepository.CheckPasswordAsync(existedUser, loginModel.Password))
            {
                return existedUser;
            }
            return null;
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(UserModel userModel)
        {
            if(userModel == null)
            {
                return null;
            }
            return await _userRepository.GetUserByEmailAsync(userModel.Email);
        }
        public async Task<bool> SignUpAsync(UserModel userModel)
        {
            if(userModel == null)
            {
                return false;
            }
            var existedUser = await _userRepository.GetUserByEmailAsync(userModel.Email);
            if (existedUser == null)
            {
                return await _userRepository.SignUpAsync(userModel.FirstName, userModel.LastName, userModel.Email, userModel.Password);
            }
            return false;
        }
        public async Task<string> GetEmailConfirmTokenAsync(ApplicationUser user)
        {
            if(user == null)
            {
                return string.Empty;
            }
            return await _userRepository.GetEmailConfirmTokenAsync(user);
        }
        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return false;
            }
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                return await _userRepository.ConfirmEmailAsync(user, token);
            }
            return false;
        }

        public async Task<bool> ResetPasswordAsync(ApplicationUser user)
        {
            var token = await _userRepository.GenerateResetPasswordTokenAsync(user);
            var newTempPassword = _passwordHelper.GenerateRandomPassword();

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
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }
            var user = await _userRepository.GetUserByIdAsync(userId);
            var roles = await _userRepository.GetRoleAsync(user);
            var role = roles.FirstOrDefault();

            return role;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                return user.UserName;
            }
            return string.Empty;
        }

        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            if(user == null)
            {
                return false;
            }
            user.IsRemoved = true; 

            return await _userRepository.UpdateUserAsync(user);
        }
    }
}
