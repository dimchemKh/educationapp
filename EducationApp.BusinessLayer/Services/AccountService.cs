using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using System.Linq;

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
        public async Task<ApplicationUser> SignInAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }
            var existedUser = await _userRepository.GetUserByEmailAsync(email);
            if (existedUser != null && await _userRepository.CheckPasswordAsync(existedUser, password))
            {
                return existedUser;
            }
            return null;
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
        public async Task<bool> SignUpAsync(string firstName, string lastName, string email, string password)
        {
            var existedUser = await _userRepository.GetUserByEmailAsync(email);
            if (existedUser == null)
            {
                return await _userRepository.SignUpAsync(firstName, lastName, email, password);
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
            var user = await _userRepository.GetUserByIdAsync(userId);
            var roles = await _userRepository.GetRoleAsync(user);
            var role = roles.FirstOrDefault();

            return role;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
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
