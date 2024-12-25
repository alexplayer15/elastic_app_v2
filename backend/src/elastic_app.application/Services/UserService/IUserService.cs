using elastic_app.application.DTOs;

namespace elastic_app.application.Services.UserService
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterRequest registrationDetails);
    }
}
