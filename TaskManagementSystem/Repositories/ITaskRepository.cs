using TaskManagementSystem.Entities;

namespace TaskManagementSystem.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();

        Task<TaskItem> GetTaskByIdAsync(string id);

        Task CreateTaskAsync(TaskItem task);

        Task UpdateTaskAsync(TaskItem task);

        Task DeleteTaskAsync(int id);

        Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId);

        Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId);

        IEnumerable<TaskItem> GetAllTasksAsQueryable();
    }
}
