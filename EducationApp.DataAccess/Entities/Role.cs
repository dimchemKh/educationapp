using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace EducationApp.DataAccessLayer.Entities
{
    public class Role : IdentityRole<long>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}