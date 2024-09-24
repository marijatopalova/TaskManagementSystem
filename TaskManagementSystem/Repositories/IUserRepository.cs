using TaskManagementSystem.Entities;

namespace TaskManagementSystem.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}
