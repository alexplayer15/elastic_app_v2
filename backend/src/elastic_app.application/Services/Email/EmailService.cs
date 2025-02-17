using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using elastic_app.domain.Models;

namespace elastic_app.application.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string registrationEmail, TokenModel tokenData)
        {
            try
            {
                using var mySmtpClient = ConfigureEmailClient();
                using var myMail = ConfigureEmailMessage(registrationEmail, tokenData.Token);

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
        private MailMessage ConfigureEmailMessage(string registrationEmail, string token)
        {
            var from = new MailAddress(_configuration["EmailSettings:FromEmail"], "Elastic App V2");
            var to = new MailAddress(registrationEmail);
            var mail = new MailMessage(from, to)
            {
                Subject = "Hi, It Worked!",
                SubjectEncoding = Encoding.UTF8,
                Body = $"It worked and if you see {token} that also worked!",
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            return mail;
        }
    }
}
