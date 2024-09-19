using Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BussinesLogic.Email
{
    public class EmailService : Contracts.Email.IEmailService
    {
        private readonly string EmailFrom;
        private readonly string EmailFromPassword;

        public EmailService(string emailFrom, string emailFromPassword)
        {
            EmailFrom = emailFrom;
            EmailFromPassword = emailFromPassword;
        }

        public async Task<bool> SendEmail(SendEmailRequest sendEmailRequest)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(EmailFrom, EmailFromPassword);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(EmailFrom);
            mailMessage.To.Add(sendEmailRequest.EmailTo);
            mailMessage.Subject = sendEmailRequest.Subject;
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append(sendEmailRequest.Body);
            mailMessage.Body = mailBody.ToString();

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
