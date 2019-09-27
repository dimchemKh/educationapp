using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<EditModel> EditUserProfileAsync(EditModelItem userModel);
        Task<UserModel> GetUserAsync(string userId, UserModel userModel);
        Task<UserModel> GetAllUsersAsync(UserModel userModel, string userName = null);
        Task<bool> DeleteUserAsync(string userId);
        Task<EditModel> AddNewUserAsync(EditModelItem user);
        Task<UserModel> BlockUserAsync(string userId, Enums.IsBlocked isBlocked, UserModel userModel);
    }
}
