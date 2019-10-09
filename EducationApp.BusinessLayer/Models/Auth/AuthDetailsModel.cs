using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Auth
{
    public class AuthDetailsModel : AuthModel
    {
        public long UserId { get; set; }
        public string UserRole { get; set; }
        public string UserName { get; set; }
    }
}
