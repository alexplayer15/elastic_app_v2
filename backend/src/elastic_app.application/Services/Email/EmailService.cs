using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace elastic_app.application.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync()
        {
            try
            {
                using var mySmtpClient = ConfigureEmailClient();
                using var myMail = ConfigureEmailMessage();

                await mySmtpClient.SendMailAsync(myMail);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("SmtpException occurred: " + ex.Message);
            }
        }
        private SmtpClient ConfigureEmailClient()
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]
                ),
                EnableSsl = true
            };

            return smtpClient;
        }
        private MailMessage ConfigureEmailMessage()
        {
            var from = new MailAddress(_configuration["EmailSettings:FromEmail"], "Your App Name");
            var to = new MailAddress("toEmail");
            var mail = new MailMessage(from, to)
            {
                Subject = "subject",
                SubjectEncoding = Encoding.UTF8,
                Body = "email-body",
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            return mail;
        }
    }
}
