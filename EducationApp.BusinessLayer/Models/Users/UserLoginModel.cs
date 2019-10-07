using EducationApp.BusinessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class UserLoginModel : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
