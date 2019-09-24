using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private SmtpClient _smtpClient = new SmtpClient("smtp.mailtrap.io", 2525);
        private NetworkCredential _networkCredential = new NetworkCredential("beb858dd98302d", "f2b28f22609ec7");
        private MailMessage _mailMessage = new MailMessage();
        public async Task SendMailAsync(ApplicationUser user, string subject, string body)
        {
            _smtpClient.Credentials = _networkCredential;
            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtpClient.UseDefaultCredentials = false;

            _mailMessage.Subject = subject;

            _mailMessage.Body = body;
            _mailMessage.IsBodyHtml = true;

            _mailMessage.From = new MailAddress("from@example.com");
            _mailMessage.To.Add(new MailAddress(user.Email));

            await _smtpClient.SendMailAsync(_mailMessage);
        }
    }
}
