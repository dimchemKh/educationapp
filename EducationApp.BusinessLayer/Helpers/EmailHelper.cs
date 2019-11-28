using EducationApp.BusinessLogic.Helpers.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EducationApp.BusinessLogic.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IConfigurationSection _configurationSection;
        private readonly MailMessage _mailMessage = new MailMessage();
        private readonly NetworkCredential _networkCredential;

        public EmailHelper(IConfiguration configuration)
        {
            _configurationSection = configuration.GetSection("EmailConfig");

            _networkCredential = new NetworkCredential(GetSectionValue("NetCredentialName"), GetSectionValue("NetCredentialPass"));
        }
        private string GetSectionValue(string section)
        {
            return _configurationSection.GetSection(section).Value;
        }
        public async Task SendMailAsync(string userEmail, string subject, string body)
        {
            if(int.TryParse(GetSectionValue("SmtpPort"), out int _port))
            {
                using (SmtpClient _smtpClient = new SmtpClient(GetSectionValue("SmtpHost"), _port))
                {
                    _smtpClient.Credentials = _networkCredential;
                    _smtpClient.EnableSsl = true;
                    _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    _mailMessage.Subject = subject;

                    _mailMessage.Body = body;
                    _mailMessage.IsBodyHtml = true;

                    _mailMessage.From = new MailAddress(GetSectionValue("TestEmail"));
                    _mailMessage.To.Add(new MailAddress(userEmail));

                    await _smtpClient.SendMailAsync(_mailMessage);
                }
            }                 
        }
    }
}
