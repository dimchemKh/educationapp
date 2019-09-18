using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(string firstName, string lastName, string email, string password);

        Task Authenticate(string email, string password);
    }
}
