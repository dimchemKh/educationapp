using EducationApp.BusinessLogic.Models.Base;
using System.Collections.Generic;

namespace EducationApp.BusinessLogic.Models.Users
{
    public class UserModel : BaseModel
    {
        public ICollection<UserModelItem> Items = new List<UserModelItem>();
    }
}
