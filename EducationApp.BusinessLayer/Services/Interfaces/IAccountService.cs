using EducationApp.BusinessLayer.Models.Auth;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> SignUpAsync(UserRegistrationModel userModel);
        Task<long> GetUserByEmailAsync(string email);
        Task<string> GetEmailConfirmTokenAsync(long userId);
        Task<AuthModel> SignInAsync(UserLoginModel loginModel);
        Task<UserRegistrationModel> ConfirmEmailAsync(string userId, string token);
        Task<bool> ResetPasswordAsync(long userId);
        //Task<string> GetRoleAsync(string userId);
        Task SendRegistrationMailAsync(long userId, string callbackUrl);
        //Task<string> GetUserNameAsync(string userId);
        Task<AuthModel> IdentifyUser(AuthModel authModel);
    }
}
