using EducationApp.BusinessLayer.Models.Base;

namespace EducationApp.BusinessLayer.Models.Users
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
