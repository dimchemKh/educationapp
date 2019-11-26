using EducationApp.BusinessLayer.Models.Base;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class UserModel : BaseModel
    {
        public ICollection<UserModelItem> Items = new List<UserModelItem>();
    }
}
