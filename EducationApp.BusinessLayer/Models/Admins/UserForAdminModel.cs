using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Admins
{
    public class UserForAdminModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Blocked { get; set; }
    }
}
