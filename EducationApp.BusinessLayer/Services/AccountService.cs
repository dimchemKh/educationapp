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
using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using System.Text;

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
                authModel.Errors.Add(Constants.Errors.EmptyPassword);
                return authModel;
            }
            var existedUser = await _userRepository.GetUserByEmailAsync(loginModel.Email);

            if (existedUser == null)
            {
                authModel.Errors.Add(Constants.Errors.UserNotFound);
                return authModel;
            }
            var result = await _userRepository.CheckPasswordAsync(existedUser, loginModel.Password);
            if (result.IsLockedOut)
            {
                authModel.Errors.Add(Constants.Errors.BlockedUser);
                return authModel;
            }
            if (!result.Succeeded)
            {
                authModel.Errors.Add(Constants.Errors.InvalidPassword);
                return authModel;
            }
            if(!await _userRepository.IsEmailConfirmedAsync(existedUser))
            {
                authModel.Errors.Add(Constants.Errors.IsConfirmedEmail);
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
                responseModel.Errors.Add(Constants.Errors.IsExistedUser);
                return responseModel;
            }
            var user = _mapperHelper.Map<UserRegistrationModel, ApplicationUser>(userRegModel);
            var builder = new StringBuilder(user.FirstName).Append(user.LastName);

            user.UserName = builder.ToString();

            var errorsList = await _userRepository.SignUpAsync(user, userRegModel.Password);
            var result = errorsList.ToList();

            if (result.Any())
            {
                responseModel.Errors = result.Select(x => x.Description).ToList();
                return responseModel;
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
                responseModel.Errors.Add(Constants.Errors.SuccessConfirmedEmail);
                return responseModel;
            }
            if(!await _userRepository.ConfirmEmailAsync(user, token))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidConfirmData);
                return responseModel;
            }
            return responseModel;
        }
        public async Task<BaseModel> ResetPasswordAsync(string email)
        {
            var responseModel = new BaseModel();
            if (string.IsNullOrWhiteSpace(email))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidEmail);
                return responseModel;
            }
            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null || user.IsRemoved.Equals(true))
            {
                responseModel.Errors.Add(Constants.Errors.FalseIdentityUser);
                return responseModel;
            };
            //if(user.LockoutEnabled)
            var token = await _userRepository.GenerateResetPasswordTokenAsync(user);
            var newTempPassword = _passwordHelper.GenerateRandomPassword();

            string message = $"This is your Temp password {newTempPassword} ! Please change his, after succesfull authorization";
            string subject = "NewTempPassword";

            _emailHelper.SendMailAsync(user.Email, subject, message);
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
