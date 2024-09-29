using TaskManagementSystem.DTOs.V2;

namespace TaskManagementSystem.Services.V2
{
    public interface ITaskServiceV2
    {
        Task<TaskDtoV2> CreateTaskAsync(TaskDtoV2 task);

        Task<List<TaskDtoV2>> GetTasksByUserIdAsync(int userId);

        Task<List<TaskDtoV2>> GetTasksByProjectIdAsync(int projectId);

        Task UpdateTaskStatusAsync(string taskId, string status);

        Task<TaskDtoV2> GetTasksByIdAsync(string taskId);

        Task<List<TaskDtoV2>> SearchTasksAsync(string keyword, string status, DateTime? dueDate, int pageNumber, int pageSize);
    }
}
