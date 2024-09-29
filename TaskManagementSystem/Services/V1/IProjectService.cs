using TaskManagementSystem.DTOs.V1;

namespace TaskManagementSystem.Services.V1
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto);

        Task<List<ProjectDto>> GetAllProjectsAsync();

        Task<ProjectDto> GetProjectByIdAsync(int projectId);

        Task AddUserToProjectAsync(int projectId, int userId);

        Task RemoveUserFromProjectAsync(int projectId, int userId);
    }
}
