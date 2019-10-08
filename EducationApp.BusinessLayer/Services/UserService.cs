using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using EducationApp.DataAccessLayer.Repository.Models;

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
        public async Task<UserModel> DeleteUserAsync(string userId)
        {
            var responseModel = new UserModel();
            var user = await _userRepository.GetUserByIdAsync(long.Parse(userId));
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
            var user = await _userRepository.GetUserByIdAsync(long.Parse(userModel.Email));
            if (!await _userRepository.CheckPasswordAsync(user, userModel.CurrentPassword) && !isAdmin)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidPassword);
                return responseModel;
            }
            if (isAdmin)
            {
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.UserName = userModel.FirstName + user.LastName;
            }
            if (!isAdmin)
            {
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Email = userModel.Email;
                user.UserName = userModel.FirstName + user.LastName;
            }
            if (!string.IsNullOrWhiteSpace(userModel.NewPassword) && !string.IsNullOrWhiteSpace(userModel.ConfirmNewPassword) 
                && userModel.NewPassword == userModel.ConfirmNewPassword && !isAdmin)
            {
                await _userRepository.ChangePasswordAsync(user, userModel.CurrentPassword, userModel.NewPassword);
            }
            await _userRepository.UpdateUserAsync(user);
            return responseModel;
        }
        // TODO: Need add filtering?
        public async Task<UserModel> GetUsersAsync(FilterUserModel filterModel)
        {
            var userModel = new UserModel();           
            var repModel = new UserRepositoryModel();
            var responseModel = new UserModelItem();

            repModel = _mapperHelper.Map(filterModel, repModel);

            var filteringUsers = await _userRepository.Filtering(repModel);

            foreach (var user in filteringUsers)
            {
                responseModel = _mapperHelper.Map(user, responseModel);
                userModel.Items.Add(responseModel);
            }

            //userModel.Items = _mapperHelper.MapEntitiesToModel(filteringUsers, userModel.Items);           

            return userModel;
        }
        public async Task<UserModel> BlockUserAsync(string userId, Enums.IsBlocked isBlocked)
        {
            var responseModel = new UserModel();
            var user = await _userRepository.GetUserByIdAsync(long.Parse(userId));
            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }
            if (isBlocked == Enums.IsBlocked.True)
            {
                user.LockoutEnabled = false;
            }
            if (isBlocked == Enums.IsBlocked.False)
            {
                user.LockoutEnabled = true;
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

            userModel.UserId = user.Id.ToString();
            userModel.FirstName = user.FirstName;
            userModel.LastName = user.LastName;
            userModel.Email = user.Email;

            return userModel;
        }
    }
}
