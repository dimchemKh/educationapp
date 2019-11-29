using EducationApp.BusinessLogic.Services.Interfaces;
using System.Threading.Tasks;
using EducationApp.BusinessLogic.Helpers.Interfaces;
using System.Linq;
using EducationApp.BusinessLogic.Models.Users;
using EducationApp.BusinessLogic.Common.Constants;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.BusinessLogic.Models.Base;
using EducationApp.BusinessLogic.Helpers.Mappers.Interfaces;
using EducationApp.BusinessLogic.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using EducationApp.BusinessLogic.Models.Configs;
using System.Text;

namespace EducationApp.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly IMapperHelper _mapperHelper;
        private readonly IConfiguration _configuration;
        private readonly IOptions<EmailConfig> _emailConfig;

        public AccountService(IUserRepository userRepository, IPasswordHelper passwordHelper, IEmailHelper emailHelper,
            IMapperHelper mapperHelper, IConfiguration configuration, IOptions<EmailConfig> emailConfig)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _emailHelper = emailHelper;
            _mapperHelper = mapperHelper;
            _configuration = configuration;
            _emailConfig = emailConfig;
        }
        public async Task<UserInfoModel> SignInAsync(UserLoginModel loginModel)
        {
            var userInfoModel = new UserInfoModel();

            if (loginModel == null 
                || string.IsNullOrWhiteSpace(loginModel.Email) 
                || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                userInfoModel.Errors.Add(Constants.Errors.PasswordEmpty);
                return userInfoModel;
            }

            var existedUser = await _userRepository.GetUserByEmailAsync(loginModel.Email);

            if (existedUser == null)
            {
                userInfoModel.Errors.Add(Constants.Errors.UserNotFound);
                return userInfoModel;
            }

            var result = await _userRepository.CheckPasswordAsync(existedUser, loginModel.Password);

            if (result.IsLockedOut)
            {
                userInfoModel.Errors.Add(Constants.Errors.UserBloced);
                return userInfoModel;
            }

            if (!result.Succeeded)
            {
                userInfoModel.Errors.Add(Constants.Errors.PasswordInvalid);
                return userInfoModel;
            }

            var confirmResult = await _userRepository.IsEmailConfirmedAsync(existedUser);

            if (!confirmResult)
            {
                userInfoModel.Errors.Add(Constants.Errors.EmailIsConfirmed);
                return userInfoModel;
            }

            var role = (await _userRepository.GetRoleAsync(existedUser)).FirstOrDefault();

            return existedUser.MapToInfoModel(userInfoModel, role);
        }
        public async Task<BaseModel> SignUpAsync(UserRegistrationModel userRegModel)
        {
            var responseModel = new BaseModel();

            if(userRegModel == null 
                || string.IsNullOrWhiteSpace(userRegModel.FirstName)
                || string.IsNullOrWhiteSpace(userRegModel.LastName)
                || string.IsNullOrWhiteSpace(userRegModel.Email))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }

            var existedUser = await _userRepository.GetUserByEmailAsync(userRegModel.Email);

            if (existedUser != null)
            {
                responseModel.Errors.Add(Constants.Errors.UserExisted);
                return responseModel;
            }

            var user = _mapperHelper.Map<UserRegistrationModel, ApplicationUser>(userRegModel);

            user.UserName = $"{user.FirstName}{user.LastName}";
            user.Image = userRegModel.Image;

            var result = (await _userRepository.SignUpAsync(user, userRegModel.Password)).ToList();

            if (result.Any())
            {
                responseModel.Errors = result.Select(x => x.Description).ToList();
            }

            return responseModel;
        }
        public async Task<UserShortModel> GetEmailConfirmTokenAsync(string email)
        {
            var userModel = new UserShortModel();
            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null)
            {
                userModel.Errors.Add(Constants.Errors.UserNotFound);
                return userModel;
            }

            userModel.UserId = user.Id;
            userModel.ConfirmToken = await _userRepository.GetEmailConfirmTokenAsync(user);

            return userModel;
        }
        public async Task<UserRegistrationModel> ConfirmEmailAsync(string userId, string token)
        {
            var responseModel = new UserRegistrationModel();

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidConfirmData);
                return responseModel;
            }

            if(!long.TryParse(userId, out long _userId))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidConfirmData);
                return responseModel;
            }

            var user = await _userRepository.GetUserByIdAsync(_userId);

            if (user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }

            if (await _userRepository.IsEmailConfirmedAsync(user))
            {
                responseModel.Errors.Add(Constants.Errors.EmailConfirmed);
                return responseModel;
            }

            if(!await _userRepository.ConfirmEmailAsync(user, token))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidConfirmData);
            }

            return responseModel;
        }
        public async Task<BaseModel> ResetPasswordAsync(string email)
        {
            var responseModel = new BaseModel();

            if (string.IsNullOrWhiteSpace(email))
            {
                responseModel.Errors.Add(Constants.Errors.EmailInvalid);
                return responseModel;
            }

            var user = await _userRepository.GetUserByEmailAsync(email);

            if(user == null || user.IsRemoved)
            {
                responseModel.Errors.Add(Constants.Errors.UserFailIdentity);
                return responseModel;
            };

            if (!user.LockoutEnd.Equals(null))
            {
                responseModel.Errors.Add(Constants.Errors.UserBloced);
                return responseModel;
            }

            var token = await _userRepository.GenerateResetPasswordTokenAsync(user);

            var newTempPassword = _passwordHelper.GenerateRandomPassword(_configuration);

            string message = $"This is your Temp password {newTempPassword} ! Please change it`s, after succesfull authorization";

            await _emailHelper.SendMailAsync(user.Email, _emailConfig.Value.SubjectRecovery, message);

            var result = await _userRepository.ResetPasswordAsync(user, token, newTempPassword);

            if (!result)
            {
                responseModel.Errors.Add(Constants.Errors.UserRemoved);
            }

            return responseModel;
        }
        public async Task SendRegistrationMailAsync(long userId, string callbackUrl)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            var body = new StringBuilder($"Hi, {user.FirstName}!" +
                $"You have been sent this email because you created an account on our website. " +
                $"Please click on <a href =\"" + callbackUrl + "\">this link</a> to confirm your email address is correct. ");

            await _emailHelper.SendMailAsync(user.Email, _emailConfig.Value.SubjectConfirmEmail, body.ToString());
        }
        public async Task<UserInfoModel> IdentifyUser(UserInfoModel userInfoModel)
        {            
            var user = await _userRepository.GetUserByIdAsync(userInfoModel.UserId);

            if(user == null)
            {
                userInfoModel.Errors.Add(Constants.Errors.UserNotFound);
                return userInfoModel;
            }

            var role = (await _userRepository.GetRoleAsync(user)).FirstOrDefault();

            return user.MapToInfoModel(userInfoModel, role);
        }

        public async Task LogOutAsync()
        {
            await _userRepository.LogOutAsync();
        }
    }    
}
