using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.Services
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(TaskDto task);

        Task<List<TaskDto>> GetTasksByUserIdAsync(int userId);

        Task<List<TaskDto>> GetTasksByProjectIdAsync(int projectId);

        Task UpdateTaskStatusAsync(string taskId, string status);

        Task<TaskDto> GetTasksByIdAsync(string taskId);
    }
}
