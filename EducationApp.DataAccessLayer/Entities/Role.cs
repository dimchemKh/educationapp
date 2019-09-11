using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class Role : IdentityRole<long>
    {
        public new int Id { get; set; }
        public new string Name { get; set; }

    }
}
