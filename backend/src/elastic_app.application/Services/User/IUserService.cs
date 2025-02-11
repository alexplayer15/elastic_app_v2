using elastic_app.application.DTOs;

namespace elastic_app.application.Services.User
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterRequest registrationDetails);
        Task<bool> VerifyEmailAsync(string emailToken);
    }
}
