using elastic_app.application.DTOs;
using elastic_app.application.Commands;
using elastic_app.application.Services.User;
using MediatR;
using elastic_app.domain.Abstractions;
using System.Text;
using elastic_app.application.Services.Email;
using elastic_app.domain.Models;
using FluentValidation;
using elastic_app.application.Validations;
using Mapster;
using elastic_app.application.Services.VerificationToken;

namespace elastic_app.application.Handlers
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequestCommand, Unit>
    {
        private readonly IUserService _userService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IEmailService _emailService;
        private readonly IVerificationTokenService _verificationTokenService;
        private readonly IValidator<RegisterRequest> _registerRequestValidator;
        public RegisterRequestHandler(IUserService userService,
            IEmailService emailService,
            ITokenProvider tokenProvider,
            IValidator<RegisterRequest> registerRequestValidator,
            IVerificationTokenService verificationTokenService)
        {
            _userService = userService;
            _emailService = emailService;
            _tokenProvider = tokenProvider;
            _registerRequestValidator = registerRequestValidator;
            _verificationTokenService = verificationTokenService;
        }
        public async Task<Unit> Handle(RegisterRequestCommand registerRequest, CancellationToken cancellationToken)
        {
            if (registerRequest == null)
            {
                throw new ArgumentNullException(nameof(registerRequest), "registration details cannot be null");
            }

            var registrationDetails = registerRequest.Adapt<RegisterRequest>();

            var validationResult = await _registerRequestValidator.ValidateAsync(registrationDetails);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                var errorMessage = string.Join("; ", errors);

                throw new InvalidOperationException(errorMessage);
            }

            var registeredUser = await _userService.RegisterUserAsync(registrationDetails);

            var tokenData = await _verificationTokenService.GenerateVerificationTokenAsync(registeredUser);

            await _emailService.SendEmailAsync(registeredUser.Email, tokenData);

            return Unit.Value;
        }
    }
}
