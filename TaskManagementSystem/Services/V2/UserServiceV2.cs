using TaskManagementSystem.DTOs.V2;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Repositories;

namespace TaskManagementSystem.Services.V2
{
    public class UserServiceV2(IUserRepository userRepository,
        IProjectRepository projectRepository) : IUserServiceV2
    {
        public async Task<UserDtoV2> CreateUserAsync(UserDtoV2 userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
            };

            await userRepository.CreateUserAsync(user);
            return userDto;
        }

        public async Task<List<UserDtoV2>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllUsersAsync();
            return users.Select(u => new UserDtoV2()
            {
                Name = u.Name,
                Email = u.Email,
                Id = u.Id
            }).ToList();
        }

        public async Task<UserDtoV2> GetUserByIdAsync(int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var userDto = new UserDtoV2
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
            };

            return userDto;
        }

        public async Task<List<UserDtoV2>> GetUsersByProjectIdAsync(int projectId)
        {
            var project = await projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            var users = project.Users.Select(pu => new UserDtoV2()
            {
                Name = pu.Name,
                Email = pu.Email,
                Id = pu.Id,
            }).ToList();

            return users;
        }
    }
}
