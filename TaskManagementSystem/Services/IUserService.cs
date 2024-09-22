using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserDto userDto);

        Task<List<UserDto>> GetAllUsersAsync();

        Task<UserDto> GetUserByIdAsync(int userId);

        Task<List<UserDto>> GetUsersByProjectIdAsync(int projectId);
    }
}
