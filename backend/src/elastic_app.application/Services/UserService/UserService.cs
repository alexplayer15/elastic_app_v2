using elastic_app.application.DTOs;
using elastic_app.domain.Models;
using elastic_app.domain.Abstractions;
using elastic_app.application.Validations;

namespace elastic_app.application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUserAsync(RegisterRequest registrationDetails)
        {
            if (registrationDetails == null)
            {
                throw new ArgumentNullException(nameof(registrationDetails));
            }

            var validator = new RegisterRequestValidation();
            var validationResult = await validator.ValidateAsync(registrationDetails);

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

            var user = new User
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
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); 
        }
    }
}