
using insightflow_users_service.src.Helpers.Requests;
using insightflow_users_service.src.Models;

namespace insightflow_users_service.src.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<(IEnumerable<User> Users, int TotalCount)> GetAllAsync(UserQuery query);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task UpdateAsync(User user);
        Task SoftDeleteAsync(Guid id);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
    }
}