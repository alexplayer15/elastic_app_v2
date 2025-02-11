using elastic_app.application.DTOs;
using elastic_app.application.Commands;
using elastic_app.application.Services.User;
using MediatR;
using System.Text;
using elastic_app.application.Services.Email;

namespace elastic_app.application
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequestCommand, Unit>
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public RegisterRequestHandler(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }
        public async Task<Unit> Handle(RegisterRequestCommand registerRequest, CancellationToken cancellationToken)
        {
            if (registerRequest == null)
            {
                throw new ArgumentNullException(nameof(registerRequest), "registration details cannot be null");
            }

            var registrationDetails = new RegisterRequest
            {
                Forename = registerRequest.Forename,
                Surname = registerRequest.Surname,
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                ReEnterPassword = registerRequest.ReEnterPassword
            };

            await _userService.RegisterUserAsync(registrationDetails);

            var verificationLink = GenerateVerificationLink(registerRequest.Email);
            var emailBody = $"<p>Thank you for registering! Please verify your email by clicking <a href='{verificationLink}'>here</a>.</p>";

            await _emailService.SendEmailAsync(registerRequest.Email, "Verify Your Email", emailBody);

            return Unit.Value;
        }

        private string GenerateVerificationLink(string email)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(email));
            return token;

        }
    }

}
