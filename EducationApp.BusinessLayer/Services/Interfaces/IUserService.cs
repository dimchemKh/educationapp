using EducationApp.BusinessLayer.Models.Admins;
using EducationApp.BusinessLayer.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> EditUserProfileAsync(RegistrationModel userModel);
        Task<RegistrationModel> GetUserAsync(string userId);
        Task<IList<UserForAdminModel>> GetAllUsersAsync();
        Task<UserForAdminModel> GetAllUsersAsync(string userName, bool blocked);
        Task<IList<OrderForAdminModel>> GetUsersOrdersAsync(string userId);
        Task<IList<OrderModel>> GetOrdersAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> AddNewUserAsync(RegisterForAdminModel user);
    }
}
