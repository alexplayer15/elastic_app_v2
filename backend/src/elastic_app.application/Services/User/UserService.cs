using elastic_app.application.DTOs;
using elastic_app.domain.Models;
using elastic_app.domain.Abstractions;
using FluentValidation;
using System.Text;

namespace elastic_app.application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegisterRequest> _registerRequestValidator;

        public UserService(IUserRepository userRepository, IValidator<RegisterRequest> registerRequestValidator)
        {
            _registerRequestValidator = registerRequestValidator;
            _userRepository = userRepository;
        }

        public async Task RegisterUserAsync(RegisterRequest registrationDetails)
        {

            if (registrationDetails == null)
            {
                throw new ArgumentNullException(nameof(registrationDetails), "registration details cannot be null");
            }

            var validationResult = await _registerRequestValidator.ValidateAsync(registrationDetails);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                var errorMessage = string.Join("; ", errors);

                throw new InvalidOperationException(errorMessage);
            }

            bool emailExists = await _userRepository.CheckEmailExistsAsync(registrationDetails.Email);
            if (emailExists)
            {
                throw new InvalidOperationException("This email is already in use");
            }

            bool usernameExists = await _userRepository.CheckUsernameExistsAsync(registrationDetails.Username);
            if (usernameExists)
            {
                throw new InvalidOperationException("This username is already in use");
            }

            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                Forename = registrationDetails.Forename,
                Surname = registrationDetails.Surname,
                Username = registrationDetails.Username,
                Email = registrationDetails.Email,
                PasswordHash = HashPassword(registrationDetails.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddUserAsync(user);  
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            var user = await _userRepository.GetUserByVerificationTokenAsync(token);

            if (user == null || user.IsEmailVerified)
            {
                return false; // Invalid or already verified token
            }

            user.IsEmailVerified = true;
            user.VerificationToken = null; // Invalidate the token after use
            await _userRepository.UpdateAsync(user);

            return true;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); 
        }
    }
}