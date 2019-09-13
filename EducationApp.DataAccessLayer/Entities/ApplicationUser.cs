using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public new string Email
        {
            get
            {
                return base.Email;
            }

            set
            {
                base.Email = value;
            }
        }

    }
}
