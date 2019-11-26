using EducationApp.BusinessLayer.Models.Base;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class UserLoginModel : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
