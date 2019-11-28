using EducationApp.BusinessLogic.Models.Base;

namespace EducationApp.BusinessLogic.Models.Users
{
    public class UserShortModel : BaseModel
    {
        public long UserId { get; set; }
        public string ConfirmToken { get; set; }
    }
}
