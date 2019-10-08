using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface IEmailHelper
    {
        Task SendMailAsync(string userEmail, string subject, string body);
    }
}
