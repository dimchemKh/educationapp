using EducationApp.BusinessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class UserInfoModel : BaseModel
    {
        public long UserId { get; set; }
        public string UserRole { get; set; }
        public string UserName { get; set; }
    }
}
