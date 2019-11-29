using EducationApp.BusinessLogic.Models.Base;

namespace EducationApp.BusinessLogic.Models.Users
{
    public class UserModelItem : BaseModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public bool IsBlocked { get; set; }
    }
}
