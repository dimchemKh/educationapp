using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLogic.Models.Filters;
using EducationApp.BusinessLogic.Models.Users;
using EducationApp.BusinessLogic.Services.Interfaces;
using EducationApp.BusinessLogic.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.BusinessLogic.Helpers.Mappers.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using EducationApp.BusinessLogic.Helpers.Mappers;

namespace EducationApp.BusinessLogic.Services
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

            var updateResult = await _userRepository.UpdateUserAsync(user);

            if (!updateResult)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
            }

            return responseModel;
        }
        public async Task<UserUpdateModel> UpdateUserProfileAsync(UserUpdateModel userModel, bool isAdmin)
        {
            var responseModel = new UserUpdateModel();

            if (string.IsNullOrWhiteSpace(userModel.CurrentPassword) && !isAdmin)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }

            var user = await _userRepository.GetUserByIdAsync(userModel.Id);

            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNotFound);
                return responseModel;
            }

            var checkPasswordResult = await _userRepository.CheckPasswordAsync(user, userModel.CurrentPassword, isAdmin);

            if (!checkPasswordResult.Succeeded)
            {
                responseModel.Errors.Add(Constants.Errors.PasswordInvalid);
                return responseModel;
            }

            user = userModel.MapToEntity(user);

            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
                return responseModel;
            }

            var errors = new List<IdentityError>();

            if (!string.IsNullOrWhiteSpace(userModel.NewPassword))
            {
                errors = (await _userRepository.ChangePasswordAsync(user, userModel.CurrentPassword, userModel.NewPassword)).ToList();
            }              

            if (errors.Any())
            {
                responseModel.Errors = errors.Select(x => x.Description).ToList();
                return responseModel;
            }

            var updateResult = await _userRepository.UpdateUserAsync(user);

            if (!updateResult)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
            } 

            return responseModel;
        }
        
        public async Task<UserModel> GetAllUsersAsync(FilterUserModel filterModel)
        {
            var userModel = new UserModel();  

            var repositoryModel = _mapperHelper.Map<FilterUserModel, DataFilter.FilterUserModel>(filterModel);

            if(repositoryModel == null)
            {
                userModel.Errors.Add(Constants.Errors.OccuredProcessing);
                return userModel;
            }

            var filteringUsers = await _userRepository.GetFilteredDataAsync(repositoryModel);

            if(filteringUsers == null)
            {
                userModel.Errors.Add(Constants.Errors.OccuredProcessing);
                return userModel;
            }

            foreach (var user in filteringUsers.Collection)
            {
                var userModelItem = user.MapToModel<UserModelItem>();

                if(userModelItem == null)
                {
                    userModel.Errors.Add(Constants.Errors.OccuredProcessing);
                    continue;
                }

                userModel.Items.Add(userModelItem);
            }

            userModel.ItemsCount = filteringUsers.CollectionCount;

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

            await _userRepository.BlockUserAsync(user, isBlocked);

            return responseModel;
        }

        public async Task<UserUpdateModel> GetOneUserAsync(string userId)
        {
            var userModel = new UserUpdateModel();

            if (string.IsNullOrWhiteSpace(userId))
            {
                userModel.Errors.Add(Constants.Errors.UserIdInvalid);
                return userModel;
            }

            if(!long.TryParse(userId, out long _userId) && _userId <= 0){
                userModel.Errors.Add(Constants.Errors.UserIdInvalid);
                return userModel;
            }

            var user = await _userRepository.GetUserByIdAsync(_userId);

            if(user == null)
            {
                userModel.Errors.Add(Constants.Errors.UserNotFound);
                return userModel;
            }

            userModel = _mapperHelper.Map<ApplicationUser, UserUpdateModel>(user);

            if(userModel == null)
            {
                userModel.Errors.Add(Constants.Errors.OccuredProcessing);
            }

            return userModel;
        }
    }
}
