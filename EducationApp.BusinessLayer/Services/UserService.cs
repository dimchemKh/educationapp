using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
namespace EducationApp.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapperHelper _mapperHelper;

        public UserService(IUserRepository userRepository, IMapperHelper mapperHelper)
        {
            _userRepository = userRepository;
            _mapperHelper = mapperHelper;
        }
        public async Task<UserModel> DeleteUserAsync(long userId)
        {
            var responseModel = new UserModel();
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }
            user.IsRemoved = true;

            await _userRepository.UpdateUserAsync(user);
            return responseModel;
        }
        public async Task<UserEditModel> EditUserProfileAsync(UserEditModel userModel, bool isAdmin = false)
        {
            var responseModel = new UserEditModel();
            var user = await _userRepository.GetUserByIdAsync(userModel.Id);
            if (string.IsNullOrWhiteSpace(userModel.CurrentPassword))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidPassword);
                return responseModel;
            }
            if (!await _userRepository.CheckPasswordAsync(user, userModel.CurrentPassword))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidPassword);
                return responseModel;
            }

            user = _mapperHelper.MapToEntity(userModel, user);
            
            if (string.IsNullOrWhiteSpace(userModel.NewPassword) && string.IsNullOrWhiteSpace(userModel.ConfirmNewPassword) 
                || userModel.NewPassword != userModel.ConfirmNewPassword && !isAdmin)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidPassword);
                return responseModel;
            }
            var result = await _userRepository.ChangePasswordAsync(user, userModel.CurrentPassword, userModel.NewPassword);
            
            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(x => responseModel.Errors.Add(x.Description));
                return responseModel;
            }
            await _userRepository.UpdateUserAsync(user);
            return responseModel;
        }
        
        public async Task<UserModel> GetUsersAsync(FilterUserModel filterModel)
        {
            var userModel = new UserModel();           
            var repositoryModel = new DataFilter.FilterUserModel();
            var userModelItem = new UserModelItem();

            repositoryModel = _mapperHelper.MapToModelItem(filterModel, repositoryModel);

            var filteringUsers = await _userRepository.Filtering(repositoryModel);
            if(filteringUsers == null)
            {
                userModel.Errors.Add(Constants.Errors.InvalidFiltteringData);
                return userModel;
            }
            foreach (var user in filteringUsers)
            {
                userModelItem = _mapperHelper.MapToModelItem(user, userModelItem);
                userModel.Items.Add(userModelItem);
            }          
            return userModel;
        }
        public async Task<UserModel> BlockUserAsync(long userId, bool isBlocked)
        {
            var responseModel = new UserModel();
            var user = await _userRepository.GetUserByIdAsync(userId);
            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }
            if (isBlocked)
            {
                user.LockoutEnabled = true;
            }
            if (!isBlocked)
            {
                user.LockoutEnabled = false;
            }
            await _userRepository.UpdateUserAsync(user);
            return responseModel;
        }

        public async Task<UserEditModel> GetUserAsync(string userId)
        {
            var userModel = new UserEditModel();

            if (string.IsNullOrWhiteSpace(userId))
            {
                userModel.Errors.Add(Constants.Errors.UserNotFound);
                return userModel;
            }
            var user = await _userRepository.GetUserByIdAsync(long.Parse(userId));
            // Need Map

            userModel = _mapperHelper.MapToModelItem(user, userModel);

            return userModel;
        }
    }
}
