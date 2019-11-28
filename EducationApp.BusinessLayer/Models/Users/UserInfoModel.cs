using EducationApp.BusinessLogic.Models.Base;

namespace EducationApp.BusinessLogic.Models.Users
{
    public class UserInfoModel : BaseModel
    {
        public long UserId { get; set; }
        public string UserRole { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
    }
}
