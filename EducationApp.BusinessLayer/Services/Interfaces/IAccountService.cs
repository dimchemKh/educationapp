using EducationApp.BusinessLogic.Models.Base;
using EducationApp.BusinessLogic.Models.Users;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogic.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseModel> SignUpAsync(UserRegistrationModel userModel);
        Task<UserShortModel> GetEmailConfirmTokenAsync(string email);
        Task<UserInfoModel> SignInAsync(UserLoginModel loginModel);
        Task<UserRegistrationModel> ConfirmEmailAsync(string userId, string token);
        Task<BaseModel> ResetPasswordAsync(string email);
        Task SendRegistrationMailAsync(long userId, string callbackUrl);
        Task<UserInfoModel> IdentifyUser(UserInfoModel authModel);
    }
}
