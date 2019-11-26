using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface IEmailHelper
    {
        Task SendMailAsync(string userEmail, string subject, string body);
    }
}
