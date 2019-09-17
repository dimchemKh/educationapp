using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Services.Interfaces
{
    public interface IAccountService
    {
        Task RegisterAsync(string firstName, string lastName, string email, string password);

        Task Authorize(string email, string password);
    }
}
