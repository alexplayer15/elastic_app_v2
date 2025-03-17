//using elastic_app.application.Services.Email;
//using elastic_app.domain.Models;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace elastic_app.unit.tests.Services.Email
//{
//    public class EmailServiceTests
//    {
//        private readonly IEmailService _emailService;
//        private readonly IConfiguration _configuration;

//        public EmailServiceTests(IConfiguration configuration)
//        {
//            _configuration = configuration;
//            _emailService = new EmailService(configuration);
//        }

//        [Fact]
//        public void SendEmailAsync_WhenISendAnEmail_ShouldSendEmail()
//        {
//            //Arrange 
//            string registrationEmail = "alexplayer15@icloud.com";
//            TokenModel tokenModel = new TokenModel();
//            tokenModel.Token = "example";

//            //Act
//            _emailService.SendEmailAsync(registrationEmail, tokenModel);

//            //Assert
//            Task.FromResult(true);
//        }
//    }
//}
