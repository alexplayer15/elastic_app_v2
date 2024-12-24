using elastic_app.api.Models;

namespace elastic_app.api.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUsernameExistsAsync(string username);
        Task AddUserAsync(User user);
    }
}
