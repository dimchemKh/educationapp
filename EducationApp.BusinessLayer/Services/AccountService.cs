using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using System.Linq;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.BusinessLayer.Models.Auth;

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
        public async Task<AuthModel> SignInAsync(UserLoginModel loginModel)
        {
            var authModel = new AuthModel();
            if (string.IsNullOrWhiteSpace(loginModel.Email) 
                || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                authModel.Errors.Add(Constants.Errors.InvalidData);
                return authModel;
            }
            var existedUser = await _userRepository.GetUserByEmailAsync(loginModel.Email);

            if (existedUser == null)
            {
                authModel.Errors.Add(Constants.Errors.UserNotFound);
                return authModel;
            }
            if(!await _userRepository.CheckPasswordAsync(existedUser, loginModel.Password))
            {
                authModel.Errors.Add(Constants.Errors.InvalidPassword);
                return authModel;
            }
            authModel.UserId = existedUser.Id.ToString();
            return authModel;
        }
        public async Task<long> GetUserByEmailAsync(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                return Constants.Errors.NotFindUserId;
            }
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user.Id;
        }
        public async Task<UserModel> SignUpAsync(UserRegistrationModel userRegModel)
        {
            var userModel = new UserModel();
            if(userRegModel == null)
            {
                userModel.Errors.Add(Constants.Errors.InvalidDataFromClient);
                return userModel;
            }

            if(string.IsNullOrWhiteSpace(userRegModel.FirstName)
                || string.IsNullOrWhiteSpace(userRegModel.LastName)
                || string.IsNullOrWhiteSpace(userRegModel.Email))
            {
                userModel.Errors.Add(Constants.Errors.InvalidData);
                return userModel;
            }
            var existedUser = await _userRepository.GetUserByEmailAsync(userRegModel.Email);

            if (existedUser == null)
            {
                await _userRepository.SignUpAsync(userRegModel.FirstName, userRegModel.LastName, userRegModel.Email, userRegModel.Password);
            }
            return userModel;
        }
        public async Task<string> GetEmailConfirmTokenAsync(long userId)
        {
            if(userId == Constants.Errors.NotFindUserId)
            {
                return string.Empty;
            }
            return await _userRepository.GetEmailConfirmTokenAsync(userId);
        }
        public async Task<UserRegistrationModel> ConfirmEmailAsync(string userId, string token)
        {
            var regModel = new UserRegistrationModel();

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                regModel.Errors.Add(Constants.Errors.InvalidIdOrToken);
                return regModel;
            }
            var user = await _userRepository.GetUserByIdAsync(long.Parse(userId));
            if (user == null)
            {
                regModel.Errors.Add(Constants.Errors.UserNotFound);
                return regModel;
            }
            if(!await _userRepository.ConfirmEmailAsync(user, token))
            {
                regModel.Errors.Add(Constants.Errors.InvalidIdOrToken);
                return regModel;
            }
            return regModel;
        }

        public async Task<bool> ResetPasswordAsync(long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var token = await _userRepository.GenerateResetPasswordTokenAsync(userId);
            var newTempPassword = _passwordHelper.GenerateRandomPassword();

            string message = $"This is your Temp password {newTempPassword} ! Please change his, after succesfull authorization";
            string subject = "NewTempPassword";
            await _emailHelper.SendMailAsync(user.Email, subject, message);

            return await _userRepository.ResetPasswordAsync(user, token, newTempPassword);
        }
        public async Task SendRegistrationMailAsync(long userId, string callbackUrl)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var body = $"Hi, {user.FirstName}!" +
                $"You have been sent this email because you created an account on our website. " +
                $"Please click on <a href =\"" + callbackUrl + "\">this link</a> to confirm your email address is correct. ";
            await _emailHelper.SendMailAsync(user.Email, "ConfirmEmail", body);
        }

        public async Task<string> GetRoleAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }
            var user = await _userRepository.GetUserByIdAsync(long.Parse(userId));
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
            var user = await _userRepository.GetUserByIdAsync(long.Parse(userId));
            if (user != null)
            {
                return user.UserName;
            }
            return string.Empty;
        }
    }    
}
