using TaskManagementSystem.DTOs.V2;

namespace TaskManagementSystem.Services.V2
{
    public interface IUserServiceV2
    {
        Task<UserDtoV2> CreateUserAsync(UserDtoV2 userDto);

        Task<List<UserDtoV2>> GetAllUsersAsync();

        Task<UserDtoV2> GetUserByIdAsync(int userId);

        Task<List<UserDtoV2>> GetUsersByProjectIdAsync(int projectId);
    }
}
