using EducationApp.BusinessLayer.Models.Base;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class UserModelItem : BaseModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsBlocked { get; set; }
    }
}
