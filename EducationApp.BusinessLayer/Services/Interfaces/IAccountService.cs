using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser> RegisterAsync(string firstName, string lastName, string email, string password);
        Task<string> GetConfirmToken(ApplicationUser user);

        Task<ApplicationUser> Authorization(string email, string password);
    }
}
