using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<UserEditModel> EditUserProfileAsync(UserEditModel userModel);
        Task<UserModel> GetUserAsync(string userId, UserModel userModel);
        Task<UserModel> GetAllUsersAsync(UserModel userModel, string userName = null);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserEditModel> AddNewUserAsync(UserEditModel user);
        Task<UserModel> BlockUserAsync(string userId, Enums.IsBlocked isBlocked, UserModel userModel);
    }
}
