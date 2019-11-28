using EducationApp.BusinessLogic.Models.Base;

namespace EducationApp.BusinessLogic.Models.Users
{
    public class UserRegistrationModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
    }
}
