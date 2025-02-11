using MediatR;

namespace elastic_app.application.Commands
{
    public class RegisterRequestCommand : IRequest<Unit>
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReEnterPassword { get; set; }
    }
}
