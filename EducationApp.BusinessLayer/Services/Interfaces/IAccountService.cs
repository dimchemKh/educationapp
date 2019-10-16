using EducationApp.BusinessLayer.Models.Auth;
using EducationApp.BusinessLayer.Models.Base;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> SignUpAsync(UserRegistrationModel userModel);
        Task<string> GetEmailConfirmTokenAsync(long userId);
        Task<AuthModel> SignInAsync(UserLoginModel loginModel);
        Task<UserRegistrationModel> ConfirmEmailAsync(string userId, string token);
        Task<BaseModel> ResetPasswordAsync(string email);
        Task SendRegistrationMailAsync(long userId, string callbackUrl);
        Task<AuthModel> IdentifyUser(AuthModel authModel);
    }
}
