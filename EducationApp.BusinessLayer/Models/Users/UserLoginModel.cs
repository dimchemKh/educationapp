using EducationApp.BusinessLogic.Models.Base;

namespace EducationApp.BusinessLogic.Models.Users
{
    public class UserLoginModel : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
