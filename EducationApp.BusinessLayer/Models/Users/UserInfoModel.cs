using EducationApp.BusinessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;
using static EducationApp.BusinessLayer.Common.Constants.Constants;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class UserInfoModel : BaseModel
    {
        public long UserId { get; set; }
        public string UserRole { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
    }
}
