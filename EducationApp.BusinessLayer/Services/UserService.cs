using System.Threading.Tasks;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EducationApp.BusinessLayer.Services
{
    // adminka + profile
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserModel> GetUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }
            var user = await _userRepository.GetUserByIdAsync(userId);


            return new UserModel() { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
        }
        public async Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            if(string.IsNullOrWhiteSpace(currentPassword) && string.IsNullOrWhiteSpace(newPassword))
            {
                return await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);
            }
            return false;
        }
        public async Task<bool> EditUserProfileAsync(UserModel userModel)
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

            return await _userRepository.UpdateUserAsync(user);
        }

    }
}
