using System.Threading.Tasks;

namespace EducationApp.BusinessLogic.Helpers.Interfaces
{
    public interface IEmailHelper
    {
        Task SendMailAsync(string userEmail, string subject, string body);
    }
}
