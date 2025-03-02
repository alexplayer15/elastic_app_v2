using elastic_app.domain.Abstractions;
using elastic_app.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elastic_app.application.Services.EmailVerification
{
    public class EmailVerificationService : IEmailVerificationService
    {
        IVerificationTokenRepository _verificationTokenRepository;
        IUserRepository _userRepository;
        public EmailVerificationService(IVerificationTokenRepository verificationTokenRepository 
            ,IUserRepository userRepository)
        {
            _verificationTokenRepository = verificationTokenRepository;
            _userRepository = userRepository;
        }

        public async Task VerifyEmailAsync(string token)
        {
            TokenModel tokenData = await _verificationTokenRepository.GetTokenData(token);

            bool tokenIsValid = !string.IsNullOrEmpty(token) && (DateTime.Now < tokenData.ExpiresAt);
            
            if(tokenIsValid)
            {
               await _userRepository.UpdateEmailVerificationAsync(tokenData.UserId);
            }
        }
    }
}
