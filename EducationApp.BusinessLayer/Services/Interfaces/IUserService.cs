using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserEditModel> UpdateUserProfileAsync(UserEditModel userModel, bool isAdmin);
        Task<UserEditModel> GetOneUserAsync(string userId);
        Task<UserModel> GetAllUsersAsync(FilterUserModel filterUserModel);
        Task<UserModel> DeleteUserAsync(long userId);
        Task<UserModel> BlockUserAsync(long userId, bool isBlocked);
    }
}
