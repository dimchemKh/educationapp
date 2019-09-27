﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Base;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.Interfaces;

namespace EducationApp.BusinessLayer.Services
{
    // adminka + profile
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailHelper _emailHelper;
        
        public UserService(IUserRepository userRepository, IEmailHelper emailHelper)
        {
            _userRepository = userRepository;
            _emailHelper = emailHelper;
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }
            var user = await _userRepository.GetUserByIdAsync(userId);
            user.IsRemoved = true;

            return await _userRepository.UpdateUserAsync(user);
        }
        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            if(string.IsNullOrWhiteSpace(currentPassword) && string.IsNullOrWhiteSpace(newPassword))
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                return await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);
            }
            return false;
        }
        public async Task<EditModel> EditUserProfileAsync(EditModelItem userModel)
        {
            var responseModel = new EditModel();
            if(userModel == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNull);
                return responseModel;
            }
            if(string.IsNullOrWhiteSpace(userModel.FirstName) 
                || string.IsNullOrWhiteSpace(userModel.LastName) 
                || string.IsNullOrWhiteSpace(userModel.Email) 
                || string.IsNullOrWhiteSpace(userModel.Password)) 
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var user = await _userRepository.GetUserByEmailAsync(userModel.Email);
            if (!await _userRepository.CheckPasswordAsync(user, userModel.Password))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidConfirmPassword);
                return responseModel;
            }
            await _userRepository.ChangePasswordAsync(user, userModel.Password, userModel.NewPassword);

            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Email = userModel.Email;
            user.UserName = userModel.FirstName + user.LastName;

            await _userRepository.UpdateUserAsync(user);

            return responseModel;
        }
        public async Task<UserModel> GetAllUsersAsync(UserModel userModel, string userName = null)
        {
            IEnumerable<ApplicationUser> existedUsers = null;

            var listUsers = await _userRepository.GetUsersInRoleAsync(Constants.Roles.User);
            if (listUsers == null)
            {
                userModel.Errors.Add(Constants.Errors.UserNull);
                return userModel;
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                existedUsers = listUsers.Where(user => user.IsRemoved == false);
            }
            if(userName != null)
            {
                existedUsers = listUsers.Where(user => user.FirstName.Contains(userName) || user.LastName.Contains(userName));
            }
            
            foreach (var user in existedUsers)
            {
                userModel.Items.Add(new UserModelItem()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    IsBlocked = user.LockoutEnabled
                });
            }
            return userModel;
        }

        public async Task<EditModel> AddNewUserAsync(EditModelItem user)
        {
            var responseModel = new EditModel();

            if(user == null)
            {
                responseModel.Errors.Add(Constants.Errors.UserNull);
                return responseModel;
            }
            if(string.IsNullOrWhiteSpace(user.FirstName)
                || string.IsNullOrWhiteSpace(user.LastName)
                || string.IsNullOrWhiteSpace(user.Email))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidData);
                return responseModel;
            }
            var newUser = new ApplicationUser()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = true                
            };

            if(!await _userRepository.CheckPasswordAsync(await _userRepository.GetUserByEmailAsync(user.Email), user.Password))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidConfirmPassword);
                return responseModel;
            }

            await _emailHelper.SendMailAsync(newUser, "YourPassword", $"Your temp password {user.NewPassword} ! Please, change him!");
            await _userRepository.AddNewUser(newUser, user.NewPassword);

            return responseModel;
        }

        public async Task<UserModel> BlockUserAsync(string userId, Enums.IsBlocked isBlocked, UserModel userModel)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            switch (isBlocked)
            {
                case Enums.IsBlocked.None:
                    break;
                case Enums.IsBlocked.True:
                    user.LockoutEnabled = false;
                    break;
                case Enums.IsBlocked.False:
                    user.LockoutEnabled = true;
                    break;
            }
            await _userRepository.UpdateUserAsync(user);
            userModel.Items.Add(new UserModelItem()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsBlocked = user.LockoutEnabled
            });

            return userModel;
        }

        public async Task<UserModel> GetUserAsync(string userId, UserModel userModel)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                userModel.Errors.Add(Constants.Errors.InvalidData);
                return userModel;
            }
            var user = await _userRepository.GetUserByIdAsync(userId);
            userModel.Items.Add(new UserModelItem()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsBlocked = user.LockoutEnabled
            });
            return userModel;
        }
    }
}
