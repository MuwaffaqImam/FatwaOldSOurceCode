using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace TeamWork.Core.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string[] toEmails, string emailSubject, string emailBody);
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(
            IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(string[] toEmails, string emailSubject, string emailBody)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.Sender);

            foreach (string toEmail in toEmails)
            {
                mailMessage.To.Add(new MailAddress(toEmail));
            }
            mailMessage.Subject = emailSubject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = emailBody;

            using (var client = new SmtpClient())
            {
                client.Credentials = new System.Net.NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
                client.Host = _emailSettings.MailServer;
                client.Port = _emailSettings.MailPort;

                try
                {
                    client.Send(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {
                    // log exception
                }
            }
            return false;
        }
    }
}