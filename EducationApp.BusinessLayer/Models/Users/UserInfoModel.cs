using EducationApp.BusinessLayer.Models.Base;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class UserInfoModel : BaseModel
    {
        public long UserId { get; set; }
        public string UserRole { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
    }
}
