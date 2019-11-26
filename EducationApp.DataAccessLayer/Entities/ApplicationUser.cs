using EducationApp.DataAccessLayer.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public bool IsRemoved { get; set; } = false;
        public string Image { get; set; }
    }
}
