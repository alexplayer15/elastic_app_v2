using elastic_app.domain.Models;

namespace elastic_app.domain.Abstractions
{
    public interface IUserRepository
    {
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUsernameExistsAsync(string username);
        Task AddUserAsync(User user);
    }
}
