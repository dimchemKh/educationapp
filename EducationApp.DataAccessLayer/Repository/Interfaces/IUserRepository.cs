using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.EFRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUser(int userId);
        Task<Role> GetUserRole(int userId);

        //IEnumerable<string> GetRoleNamesByUserId(string userId);
        //IEnumerable<ApplicationUser> GetUsersByRoleName(string roleName);
        Task SignUpAsync(string firstName, string lastName, string email, string password);
        Task SignInAsync(string email, string password);
        Task ChangePassword(string email);
        Task<bool> ConfirmEmail(bool confirm);
        Task<ApplicationUser> UpdateUser(int userId);
        Task<ApplicationUser> DeleteUser(int userId);
    }
}
