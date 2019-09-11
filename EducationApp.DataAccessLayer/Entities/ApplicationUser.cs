using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class ApplicationUser : Base.BaseEntity
    {
        public override int Id { get; set; }
        public override DateTime CreationDate { get; set; }
        public override bool IsRemoved { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal PasswordHash { get; set; }
        public string Email { get; set; }        
    }
}
