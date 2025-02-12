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
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUserAsync(RegisterRequest registrationDetails)
        {

            if (registrationDetails == null)
            {
                throw new ArgumentNullException(nameof(registrationDetails), "registration details cannot be null");
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
                EmailVerified = false,
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