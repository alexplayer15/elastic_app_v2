using MediatR;

namespace elastic_app.application.Commands
{
    public class EmailVerificationCommand : IRequest<Unit>
    {
        public string Token { get; }

        public EmailVerificationCommand(string token)
        {
            Token = token;
        }
    }
}
