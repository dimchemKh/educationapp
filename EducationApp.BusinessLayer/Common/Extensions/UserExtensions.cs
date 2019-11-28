using EducationApp.BusinessLogic.Models.Users;
using EducationApp.DataAccessLayer.Entities;

namespace EducationApp.BusinessLogic.Common.Extensions
{
    public static class UserExtensions
    {
        public static UserInfoModel MapToInfoModel(this ApplicationUser user, UserInfoModel userInfoModel, string role)
        {
            userInfoModel.UserId = user.Id;
            userInfoModel.UserName = $"{user.FirstName} {user.LastName}";
            userInfoModel.UserRole = role;
            userInfoModel.Image = user.Image;

            return userInfoModel;
        }
    }
}
