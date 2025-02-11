using elastic_app.application.DTOs;
using elastic_app.application.Commands;
using elastic_app.application.Services.User;
using MediatR;
using elastic_app.domain.Abstractions;

namespace elastic_app.application
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequestCommand, Unit>
    {
        private readonly IUserService _userService;
        private readonly ITokenProvider _tokenProvider;

        public RegisterRequestHandler(IUserService userService, ITokenProvider tokenProvider)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
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

            return Unit.Value;
        }
    }
}
