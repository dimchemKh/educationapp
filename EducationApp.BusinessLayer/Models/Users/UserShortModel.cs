using EducationApp.BusinessLayer.Models.Base;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class UserShortModel : BaseModel
    {
        public long UserId { get; set; }
        public string ConfirmToken { get; set; }
    }
}
