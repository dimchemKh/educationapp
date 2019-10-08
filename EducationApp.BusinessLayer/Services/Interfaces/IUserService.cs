using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserEditModel> EditUserProfileAsync(UserEditModel userModel, bool isAdmin = false);
        Task<UserEditModel> GetUserAsync(string userId);
        Task<UserModel> GetUsersAsync(FilterUserModel filterUserModel);
        Task<UserModel> DeleteUserAsync(string userId);
        Task<UserModel> BlockUserAsync(string userId, Enums.IsBlocked isBlocked);
    }
}
