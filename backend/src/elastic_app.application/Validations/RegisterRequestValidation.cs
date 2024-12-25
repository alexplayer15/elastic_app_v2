using FluentValidation;
using elastic_app.application.DTOs;

namespace elastic_app.application.Validations
{
    public class RegisterRequestValidation : AbstractValidator<RegisterRequest>
    {
        //Think about adding in error messages

        public RegisterRequestValidation()
        {
            RulesForForename();
            RulesForSurname();
            RulesForUsername();
            RulesForEmail();
            RulesForPassword();
            RulesForReEnterPassWord();
        }
        private void RulesForForename()
        {
            RuleFor(r => r.Forename)
                .NotEmpty().WithMessage("Forename cannot be empty")
                .Matches(@"^[\p{L}\p{M}'-]+$").WithMessage("Forename contains invalid characters.")
                .Length(2, 50).WithMessage("Forename must be between 2 and 50 characters.");
        }

        private void RulesForSurname()
        {
            RuleFor(r =>r.Surname)
                .NotEmpty().WithMessage("Surname cannot be empty")
                .Matches(@"^[\p{L}\p{M}'-]+$").WithMessage("Surname contains invalid characters.")
                .Length(2, 100).WithMessage("Surname must be between 2 and 100 characters.");
        }

        private void RulesForUsername()
        {
            RuleFor(r =>r.Username)
                .NotEmpty().WithMessage("Username cannot be empty")
                .Matches(@"^[\w-]+$").WithMessage("Username contains invalid characters.")
                .Length(4, 20).WithMessage("Username must be between 4 and 20 characters.");
        }

        private void RulesForEmail()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email address cannot be empty.")
                .Matches(@"^[^\s@]+@[^\s@]+\.[^\s@]+$").WithMessage("Email address is not in a valid format.");
        }

        private void RulesForPassword()
        {
            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .Length(8, 15).WithMessage("Password must be between 8 and 15 characters.")
                .Matches(@"^(?=(.*[A-Z]){2,})(?=(.*\d){2,}).*$")
                .WithMessage("Password must contain at least 2 uppercase letters and 2 numbers.");
        }
        private void RulesForReEnterPassWord()
        {
            RuleFor(r => r.ReEnterPassword)
                .NotEmpty().WithMessage("Password cannot be empty")
                .Equal(r => r.Password).WithMessage("Re-entered password must be the same as the original password");
        }
    }
}
