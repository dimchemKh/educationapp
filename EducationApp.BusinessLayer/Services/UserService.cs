using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.BusinessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;
using DataFilter = EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.BusinessLayer.Helpers.Mappers.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

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
            var updateResult = await _userRepository.UpdateUserAsync(user);
            if (!updateResult)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
            }
            return responseModel;
        }
        public async Task<UserEditModel> UpdateUserProfileAsync(UserEditModel userModel, bool isAdmin)
        {
            var responseModel = new UserEditModel();
            if (string.IsNullOrWhiteSpace(userModel.CurrentPassword))
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
            var checkPasswordResult = await _userRepository.CheckPasswordAsync(user, userModel.CurrentPassword);
            if (!checkPasswordResult.Succeeded)
            {
                responseModel.Errors.Add(Constants.Errors.InvalidPassword);
                return responseModel;
            }
            user = _mapperHelper.Map<UserEditModel, ApplicationUser>(userModel);
            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.OccuredProcessing);
                return responseModel;
            }
            var errors = new List<IdentityError>();
            if (!isAdmin)
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
                userModel.Errors.Add(Constants.Errors.InvalidFiltteringData);
                return userModel;
            }
            foreach (var user in filteringUsers)
            {
                var userModelItem = _mapperHelper.Map<ApplicationUser, UserModelItem>(user);
                if(userModelItem == null)
                {
                    userModel.Errors.Add(Constants.Errors.OccuredProcessing);
                    continue;
                }
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
            await _userRepository.BlockUserAsync(user, isBlocked);
            return responseModel;
        }

        public async Task<UserEditModel> GetOneUserAsync(string userId)
        {
            var userModel = new UserEditModel();

            if (string.IsNullOrWhiteSpace(userId))
            {
                userModel.Errors.Add(Constants.Errors.InvalidUserId);
                return userModel;
            }
            if(!long.TryParse(userId, out long _userId) && _userId <= 0){
                userModel.Errors.Add(Constants.Errors.InvalidUserId);
                return userModel;
            }
            var user = await _userRepository.GetUserByIdAsync(_userId);
            if(user == null)
            {
                userModel.Errors.Add(Constants.Errors.UserNotFound);
                return userModel;
            }
            userModel = _mapperHelper.Map<ApplicationUser, UserEditModel>(user);
            if(userModel == null)
            {
                userModel.Errors.Add(Constants.Errors.OccuredProcessing);
            }
            return userModel;
        }
    }
}
