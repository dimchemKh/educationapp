using EducationApp.BusinessLogic.Models.Auth;
using EducationApp.BusinessLogic.Models.Users;
using EducationApp.Presentation.Common;
using Microsoft.Extensions.Options;

namespace EducationApp.Presentation.Helper.Interfaces
{
    public interface IJwtHelper
    {
        UserInfoModel ValidateData(string token);
        AuthModel Generate(UserInfoModel userInfoModel, IOptions<AuthConfig> configOptions);
    }
}
