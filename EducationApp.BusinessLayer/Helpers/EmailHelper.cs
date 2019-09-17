using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers
{
    public class EmailHelper /*: IEmailSender*/
    {
        SmtpClient client = new SmtpClient("http://localhost", 52196);

        public async Task SendAsync()
        {
            var code = await UserManager<ApplicationUser>.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 25);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(credentials, credentials);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.Body = "Please confirm your account by clicking this link: <a href=\""
                                                    + callbackUrl + "\">link</a>";

            mail.From = new MailAddress(Address, App);
            mail.To.Add(new MailAddress(user.Email));
            //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

            smtpClient.Send(mail);
        }
        
        //public Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    return Task.CompletedTask;
        //}
    }
}
