using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers
{
    public class EmailHelper /*: IEmailSender*/
    {      
        public async Task SendAsync(ApplicationUser user, string message)
        {

            SmtpClient smtpClient = new SmtpClient("http://localhost", 52196);

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(user.UserName, user.PasswordHash);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.Body = message;

            //mail.Body = "Please confirm your account by clicking this link: <a href=\""
            //                                    + callbackUrl + "\">link</a>";
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("whoever@me.com");
            mail.To.Add(new MailAddress(user.Email));

            await smtpClient.SendMailAsync(mail);
        }
    }
}
