using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.EFRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<string> GetRoleNamesByUserId(string userId);
        IEnumerable<ApplicationUser> GetUsersByRoleName(string roleName);

    }
}
