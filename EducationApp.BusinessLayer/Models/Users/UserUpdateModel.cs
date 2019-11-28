using EducationApp.BusinessLogic.Models.Base;

namespace EducationApp.BusinessLogic.Models.Users
{
    public class UserUpdateModel : BaseModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string Image { get; set; }
    }
}
