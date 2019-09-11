using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class UserRole
    {
        public Role RoleId { get; set; }
        public ApplicationUser UserId { get; set; }
    }
}
