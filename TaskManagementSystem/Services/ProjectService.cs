using TaskManagementSystem.DTOs;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Repositories;

namespace TaskManagementSystem.Services
{
    public class ProjectService(IProjectRepository projectRepository,
        IUserRepository userRepository) : IProjectService
    {
        public async Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto)
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description
            };

            await projectRepository.CreateProjectAsync(project);
            return projectDto;
        }

        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await projectRepository.GetAllProjectsAsync();
            return projects.Select(x => new ProjectDto
            {
                Name = x.Name,
                Description = x.Description,
                EndDate = x.EndDate,
                Id = x.Id,
                StartDate = x.StartDate
            }).ToList();
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int projectId)
        {
            var project = await projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            var projectDto = new ProjectDto()
            {
                Name = project.Name,
                Description = project.Description,
                EndDate = project.EndDate,
                Id = project.Id
            };

            return projectDto;
        }

        public async Task AddUserToProjectAsync(int projectId, int userId)
        {
            var project = await projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            if (project.Users.Any(u => u.Id == userId))
            {
                throw new ArgumentException("User is already part of the project");
            }

            project.Users.Add(user);

            await projectRepository.UpdateProjectAsync(project);
        }

        public async Task RemoveUserFromProjectAsync(int projectId, int userId)
        {
            var project = await projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found");
            }

            var projectUser = project.Users.FirstOrDefault(u => u.Id == userId);
            if (projectUser == null)
            {
                throw new ArgumentException("User is not part of the project");
            }

            project.Users.Remove(projectUser);
            await projectRepository.UpdateProjectAsync(project);
        }
    }
}
