using EducationApp.BusinessLogic.Helpers.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EducationApp.BusinessLogic.Models.Configs;
using Microsoft.Extensions.Options;

namespace EducationApp.BusinessLogic.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IOptions<EmailConfig> _emailConfig;
        private readonly MailMessage _mailMessage = new MailMessage();
        private readonly NetworkCredential _networkCredential;

        public EmailHelper(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig;


            _networkCredential = new NetworkCredential(_emailConfig.Value.NetCredentialName, _emailConfig.Value.NetCredentialPass);
        }
        public async Task SendMailAsync(string userEmail, string subject, string body)
        {

            using (SmtpClient _smtpClient = new SmtpClient(_emailConfig.Value.SmtpHost, _emailConfig.Value.SmtpPort))
            {
                _smtpClient.Credentials = _networkCredential;
                _smtpClient.EnableSsl = true;
                _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                _mailMessage.Subject = subject;

                _mailMessage.Body = body;
                _mailMessage.IsBodyHtml = true;

                _mailMessage.From = new MailAddress(_emailConfig.Value.TestEmail);
                _mailMessage.To.Add(new MailAddress(userEmail));

                await _smtpClient.SendMailAsync(_mailMessage);
            }
                           
        }
    }
}
