using EducationApp.BusinessLogic.Models.Filters;
using EducationApp.BusinessLogic.Models.Users;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserUpdateModel> UpdateUserProfileAsync(UserUpdateModel userModel, bool isAdmin);
        Task<UserUpdateModel> GetOneUserAsync(string userId);
        Task<UserModel> GetAllUsersAsync(FilterUserModel filterUserModel);
        Task<UserModel> DeleteUserAsync(long userId);
        Task<UserModel> BlockUserAsync(long userId, bool isBlocked);
    }
}
