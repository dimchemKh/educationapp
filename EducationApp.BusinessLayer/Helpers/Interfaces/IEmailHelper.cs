using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface IEmailHelper
    {
        Task SendMailAsync(ApplicationUser user, string subject, string body);
    }
}
