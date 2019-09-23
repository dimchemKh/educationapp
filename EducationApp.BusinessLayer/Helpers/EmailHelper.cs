using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers
{
    public class EmailHelper /*: IEmailSender*/
    {      
        public async Task SendAsync(ApplicationUser user, string message)
        {

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential("dikur4051996@gmail.com", "0405Gfhreh1996zxc123");
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Timeout = 20000;

            MailMessage mail = new MailMessage();
            mail.Body = message;
            mail.IsBodyHtml = true;

            mail.From = new MailAddress("dikur4051996@gmail.com");
            mail.To.Add(new MailAddress(user.Email));

            await smtpClient.SendMailAsync(mail);
        }
    }
}
