using elastic_app.application.DTOs;
using elastic_app.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elastic_app.application.Services.VerificationToken
{
    public interface IVerificationTokenService
    {
        Task<TokenModel> GenerateVerificationTokenAsync(UserModel registeredUser);
    }
}
