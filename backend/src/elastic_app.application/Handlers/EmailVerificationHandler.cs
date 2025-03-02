using elastic_app.application.Commands;
using elastic_app.application.Services.EmailVerification;
using MediatR;

namespace elastic_app.application.Handlers
{
    public class EmailVerificationHandler : IRequestHandler<EmailVerificationCommand, Unit>
    {
        private readonly IEmailVerificationService _emailVerificationService;

        public EmailVerificationHandler(IEmailVerificationService emailVerificationService)
        {
            _emailVerificationService = emailVerificationService;
        }
        public async Task<Unit> Handle(EmailVerificationCommand request, CancellationToken cancellationToken)
        {
            await _emailVerificationService.VerifyEmailAsync(request.Token);

            return Unit.Value;
        }
    }
}

