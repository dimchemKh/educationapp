using EducationApp.BusinessLayer.Models.Auth;
using EducationApp.BusinessLayer.Models.Users;
using EducationApp.PresentationLayer.Common;
using Microsoft.Extensions.Options;

namespace EducationApp.PresentationLayer.Helper.Interfaces
{
    public interface IJwtHelper
    {
        UserInfoModel ValidateData(string token);
        AuthModel Generate(UserInfoModel userInfoModel, IOptions<Config> configOptions);
    }
}
