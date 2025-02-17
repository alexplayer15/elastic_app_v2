using elastic_app.application.DTOs;
using elastic_app.domain.Models;
using elastic_app.domain.Abstractions;
using System.Security.Cryptography;

namespace elastic_app.application.Services.VerificationToken
{
    public class VerificationTokenService : IVerificationTokenService
    {
        private readonly IVerificationTokenRepository _verificationTokenRepository;

        public VerificationTokenService(IVerificationTokenRepository verificationTokenRepository)
        {
            _verificationTokenRepository = verificationTokenRepository;
        }
        public async Task<TokenModel> GenerateVerificationTokenAsync(UserModel registeredUser)
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(32); 
            var tokenString = Convert.ToBase64String(tokenBytes);

            var token = new TokenModel
            {
                Id = Guid.NewGuid(),
                UserId = registeredUser.Id,
                Token = tokenString,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _verificationTokenRepository.AddTokenAsync(token);

            return token;
        }
    }
}
