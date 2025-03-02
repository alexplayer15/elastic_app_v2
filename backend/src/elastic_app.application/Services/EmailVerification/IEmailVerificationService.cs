using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elastic_app.application.Services.EmailVerification
{
    public interface IEmailVerificationService
    {
        Task VerifyEmailAsync(string token);
    }
}
