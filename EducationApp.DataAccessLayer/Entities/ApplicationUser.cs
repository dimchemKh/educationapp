using Microsoft.AspNetCore.Identity;

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
