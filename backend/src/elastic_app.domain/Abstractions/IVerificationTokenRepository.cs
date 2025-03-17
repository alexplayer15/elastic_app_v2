using System;
using elastic_app.domain.Models;

namespace elastic_app.domain.Abstractions
{
    public interface IVerificationTokenRepository
    {
        Task AddTokenAsync(TokenModel token);
        Task<TokenModel> GetTokenData(string token);
    }
}
