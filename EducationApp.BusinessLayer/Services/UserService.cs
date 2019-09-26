using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.Admins;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;

namespace EducationApp.BusinessLayer.Services
{
    // adminka + profile
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IEmailHelper _emailHelper;
        private IList<UserForAdminModel> _usersList;
        
        public UserService(IUserRepository userRepository, IEmailHelper emailHelper, IPasswordHelper passwordHelper)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
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

        public async Task<RegistrationModel> GetUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }
            var user = await _userRepository.GetUserByIdAsync(userId);


            return new RegistrationModel() { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
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
        public async Task<bool> EditUserProfileAsync(RegistrationModel userModel)
        {
            if(userModel == null)
            {
                return false;
            }
            if(string.IsNullOrWhiteSpace(userModel.FirstName) 
                || string.IsNullOrWhiteSpace(userModel.LastName) 
                || string.IsNullOrWhiteSpace(userModel.Email) 
                || string.IsNullOrWhiteSpace(userModel.Password)) 
            {
                return false;
            }
            var user = await _userRepository.GetUserByEmailAsync(userModel.Email);
            if (!await _userRepository.CheckPasswordAsync(user, userModel.Password))
            {
                return false;
            }
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Email = userModel.Email;
            user.UserName = userModel.Email.Substring(0, userModel.Email.IndexOf("@"));

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<IList<UserForAdminModel>> GetAllUsersAsync()
        {
            _usersList = new List<UserForAdminModel>();

            var listUsers = await _userRepository.GetUsersInRoleAsync(Constants.Roles.User);
            if (listUsers == null)
            {
                return null;
            }
            var existedUsers = listUsers.Where(user => user.IsRemoved == false);

            foreach (var user in existedUsers)
            {
                _usersList.Add(new UserForAdminModel()
                {
                    Id = Int32.Parse(user.Id.ToString()),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Blocked = user.LockoutEnabled
                });
            }

            return _usersList;
        }
        public async Task<UserForAdminModel> GetAllUsersAsync(string userName, bool blocked)
        {
            var listUsers = await _userRepository.GetUsersInRoleAsync(Constants.Roles.User);
            if (listUsers == null)
            {
                return null;
            }
            var existedUsers = listUsers.FirstOrDefault(user => user.IsRemoved == false && user.UserName == userName);

            return new UserForAdminModel() {
                Id = Int32.Parse(existedUsers.Id.ToString()),
                FirstName = existedUsers.FirstName,
                LastName = existedUsers.LastName,
                Email = existedUsers.Email,
                UserName = existedUsers.UserName
            };
        }
        public async Task<bool> AddNewUserAsync(RegisterForAdminModel user)
        {
            if(user == null)
            {
                return false;
            }
            if(string.IsNullOrWhiteSpace(user.FirstName)
                || string.IsNullOrWhiteSpace(user.LastName)
                || string.IsNullOrWhiteSpace(user.Email))
            {
                return false;
            }
            var newUser = new ApplicationUser()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            var newPassword = _passwordHelper.GenerateRandomPassword();
            await _emailHelper.SendMailAsync(newUser, "YourPassword", $"Your temp password {newPassword} ! Please, change him!");
            return await _userRepository.AddNewUser(newUser, newPassword);
        }

        public Task<IList<OrderForAdminModel>> GetUsersOrdersAsync(string userId) => throw new NotImplementedException();
        public Task<IList<OrderModel>> GetOrdersAsync(string userId) => throw new NotImplementedException();
    }
}
