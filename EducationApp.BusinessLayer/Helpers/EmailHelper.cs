using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly SmtpClient _smtpClient = new SmtpClient(Constants.SmtpSettings.SmtpHost, Constants.SmtpSettings.SmtpPort);
        private readonly NetworkCredential _networkCredential = new NetworkCredential(Constants.SmtpSettings.NetCredentialName, Constants.SmtpSettings.NetCredentialPass);
        private readonly MailMessage _mailMessage = new MailMessage();
        public async Task SendMailAsync(string userEmail, string subject, string body)
        {
            _smtpClient.Credentials = _networkCredential;
            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            _mailMessage.Subject = subject;

            _mailMessage.Body = body;
            _mailMessage.IsBodyHtml = true;

            _mailMessage.From = new MailAddress(Constants.SmtpSettings.TestEmail);
            _mailMessage.To.Add(new MailAddress(userEmail));

            await _smtpClient.SendMailAsync(_mailMessage);
        }
    }
}
