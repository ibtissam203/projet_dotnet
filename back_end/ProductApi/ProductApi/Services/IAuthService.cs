using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IAuthService
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> AuthenticateAsync(string email, string password);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
    }
}
