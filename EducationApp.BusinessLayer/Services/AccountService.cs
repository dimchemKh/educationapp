using EducationApp.BusinessLayer.Services.Interfaces;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using System.Linq;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Common.Constants;
using EducationApp.BusinessLayer.Models.Auth;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.BusinessLayer.Models.Base;

namespace EducationApp.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly IMapperHelper _mapperHelper;

        public AccountService(IUserRepository userRepository, IPasswordHelper passwordHelper, IEmailHelper emailHelper, IMapperHelper mapperHelper)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _emailHelper = emailHelper;
            _mapperHelper = mapperHelper;
        }
        public async Task<AuthModel> SignInAsync(UserLoginModel loginModel)
        {
            var authModel = new AuthModel();
            if (authModel == null 
                || string.IsNullOrWhiteSpace(loginModel.Email) 
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
            authModel.UserId = existedUser.Id;
            var roles = await _userRepository.GetRoleAsync(existedUser);
            authModel.UserRole = roles.FirstOrDefault();
            authModel.UserName = string.Concat(existedUser.FirstName, " ", existedUser.LastName);

            return authModel;
        }
        public async Task<BaseModel> SignUpAsync(UserRegistrationModel userRegModel)
        {
            var userModel = new BaseModel();
            if(userRegModel == null 
                || string.IsNullOrWhiteSpace(userRegModel.FirstName)
                || string.IsNullOrWhiteSpace(userRegModel.LastName)
                || string.IsNullOrWhiteSpace(userRegModel.Email))
            {
                userModel.Errors.Add(Constants.Errors.InvalidData);
                return userModel;
            }
            var existedUser = await _userRepository.GetUserByEmailAsync(userRegModel.Email);

            if (existedUser != null)
            {
                userModel.Errors.Add(Constants.Errors.IsExistedUser);
                return userModel;
            }
            var user = _mapperHelper.MapToModelItem<UserRegistrationModel, ApplicationUser>(userRegModel);
            
            await _userRepository.SignUpAsync(user, userRegModel.Password);
            return userModel;
        }
        public async Task<string> GetEmailConfirmTokenAsync(long userId)
        {
            return await _userRepository.GetEmailConfirmTokenAsync(userId);
        }
        public async Task<UserRegistrationModel> ConfirmEmailAsync(string userId, string token)
        {
            var responseModel = new UserRegistrationModel();

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                responseModel.Errors.Add(Constants.Errors.EmptyToken);
                return responseModel;
            }
            var user = await _userRepository.GetUserByIdAsync(long.Parse(userId));
            if (user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }
            if(!await _userRepository.ConfirmEmailAsync(user, token))
            {
                responseModel.Errors.Add(Constants.Errors.EmptyToken);
                return responseModel;
            }
            return responseModel;
        }

        public async Task<BaseModel> ResetPasswordAsync(string email)
        {
            var responseModel = new BaseModel();

            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            };
            var token = await _userRepository.GenerateResetPasswordTokenAsync(user.Id);
            var newTempPassword = _passwordHelper.GenerateRandomPassword();

            string message = $"This is your Temp password {newTempPassword} ! Please change his, after succesfull authorization";
            string subject = "NewTempPassword";

            await _emailHelper.SendMailAsync(user.Email, subject, message);
            if(!await _userRepository.ResetPasswordAsync(user, token, newTempPassword))
            {
                responseModel.Errors.Add(Constants.Errors.RemovedUser);
                return responseModel;
            }
            return responseModel;
        }
        public async Task SendRegistrationMailAsync(long userId, string callbackUrl)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var body = $"Hi, {user.FirstName}!" +
                $"You have been sent this email because you created an account on our website. " +
                $"Please click on <a href =\"" + callbackUrl + "\">this link</a> to confirm your email address is correct. ";
            await _emailHelper.SendMailAsync(user.Email, "ConfirmEmail", body);
        }
        public async Task<AuthModel> IdentifyUser(AuthModel authModel)
        {            
            var user = await _userRepository.GetUserByIdAsync(authModel.UserId);
            if(user == null)
            {
                authModel.Errors.Add(Constants.Errors.UserNotFound);
                return authModel;
            }
            var roles = await _userRepository.GetRoleAsync(user);
            authModel.UserRole = roles.FirstOrDefault();
            authModel.UserName = string.Concat(user.FirstName, " ", user.LastName);

            return authModel;
        }
    }    
}
