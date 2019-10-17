using EducationApp.BusinessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class UserShortModel : BaseModel
    {
        public long UserId { get; set; }
        public string ConfirmToken { get; set; }
    }
}
