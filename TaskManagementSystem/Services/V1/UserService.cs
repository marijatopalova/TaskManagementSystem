using TaskManagementSystem.DTOs.V1;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Repositories;

namespace TaskManagementSystem.Services.V1
{
    public class UserService(IUserRepository userRepository,
        IProjectRepository projectRepository) : IUserService
    {
        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
            };

            await userRepository.CreateUserAsync(user);
            return userDto;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllUsersAsync();
            return users.Select(u => new UserDto()
            {
                Name = u.Name,
                Email = u.Email,
                Id = u.Id
            }).ToList();
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var userDto = new UserDto
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
            };

            return userDto;
        }

        public async Task<List<UserDto>> GetUsersByProjectIdAsync(int projectId)
        {
            var project = await projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            var users = project.Users.Select(pu => new UserDto()
            {
                Name = pu.Name,
                Email = pu.Email,
                Id = pu.Id,
            }).ToList();

            return users;
        }
    }
}
