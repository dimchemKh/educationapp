using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);

    }
}
