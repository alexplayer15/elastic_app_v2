using System.Net.Mail;
using System.Net;
using System.Text;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using elastic_app.domain.Models;
using System.Text.Json;

namespace elastic_app.application.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonSecretsManager _secretsManager;

        public EmailService(IConfiguration configuration, IAmazonSecretsManager secretsManager)
        {
            _configuration = configuration;
            _secretsManager = secretsManager;
        }
        public async Task SendEmailAsync(string registrationEmail, TokenModel tokenData)
        {
            try
            {
                using var mySmtpClient = await ConfigureEmailClient();
                using var myMail = ConfigureEmailMessage(registrationEmail, tokenData.Token);

                await mySmtpClient.SendMailAsync(myMail);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException("SmtpException occurred: " + ex.Message);
            }
        }
        private async Task<SmtpClient> ConfigureEmailClient()
        {
            var secretEmailCredentials = await GetEmailCredentials();
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                Credentials = new NetworkCredential(
                    secretEmailCredentials.Username,
                    secretEmailCredentials.Password
                ),
                EnableSsl = true
            };

            return smtpClient;
        }
        private MailMessage ConfigureEmailMessage(string registrationEmail, string token)
        {
            string baseUrl = "http://localhost:8081/api/verify-email";
            string verificationLink = $"{baseUrl}?token={Uri.EscapeDataString(token)}";

            var from = new MailAddress(_configuration["EmailSettings:FromEmail"], "Elastic App V2");
            var to = new MailAddress(registrationEmail);
            var mail = new MailMessage(from, to)
            {
                Subject = "Hi, It Worked!",
                SubjectEncoding = Encoding.UTF8,
                Body = $"It worked and if you see {verificationLink} that also worked!",
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            return mail;
        }

        private async Task<SecretModel> GetEmailCredentials()
        {
            string secretName = "SESEmailCredentials";

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT",
            };

            GetSecretValueResponse response;

            try
            {
                response = await _secretsManager.GetSecretValueAsync(request);

                return JsonSerializer.Deserialize<SecretModel>(response.SecretString);
            }
            catch(InvalidRequestException e)
            {
                throw new Exception("Secret you have searched for was not valid", e);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to acquire secret from secret manager", e);
            }
        }
    }

    public class SecretModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
