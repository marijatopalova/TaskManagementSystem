using TaskManagementSystem.DTOs.V2;

namespace TaskManagementSystem.Services.V2
{
    public interface IProjectServiceV2
    {
        Task<ProjectDtoV2> CreateProjectAsync(ProjectDtoV2 projectDto);

        Task<List<ProjectDtoV2>> GetAllProjectsAsync();

        Task<ProjectDtoV2> GetProjectByIdAsync(int projectId);

        Task AddUserToProjectAsync(int projectId, int userId);

        Task RemoveUserFromProjectAsync(int projectId, int userId);
    }
}
