using elastic_app.application.DTOs;
using elastic_app.domain.Models;

namespace elastic_app.application.Services.User
{
    public interface IUserService
    {
        Task<UserModel> RegisterUserAsync(RegisterRequest registrationDetails);
    }
}
