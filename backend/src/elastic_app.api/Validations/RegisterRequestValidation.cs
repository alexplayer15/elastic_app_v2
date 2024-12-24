using FluentValidation;
using elastic_app.api.DTOs;

namespace elastic_app.api.Validations
{
    public class RegisterRequestValidation : AbstractValidator<RegisterRequest>
    {
        public void RulesForFirstName()
        {
            RuleFor(r => r.Forename)
                .NotEmpty().WithMessage("Firstname cannot be empty")
                .Matches(@"^[\p{L}\p{M}'-]+$").WithMessage("First name contains invalid characters.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");
        }
    }
}
