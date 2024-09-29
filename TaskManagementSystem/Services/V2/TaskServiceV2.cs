using TaskManagementSystem.DTOs.V2;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Repositories;
using TaskManagementSystem.Services.V2;

namespace TaskManagementSystem.Services.V2
{
    public class TaskServiceV2(ITaskRepository taskRepository, 
        IUserRepository userRepository, 
        IProjectRepository projectRepository) : ITaskServiceV2
    {
        public async Task<TaskDtoV2> CreateTaskAsync(TaskDtoV2 taskDto)
        {
            var user = await userRepository.GetUserByIdAsync(taskDto.UserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var project = await projectRepository.GetProjectByIdAsync(taskDto.ProjectId);
            if (project == null) 
            {
                throw new ArgumentException("Project not found");
            }

            if(!project.Users.Any(x => x.Id == taskDto.UserId))
            {
                throw new ArgumentException("User is not part of the project");
            }

            var task = new TaskItem
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                UserId = taskDto.UserId,
                ProjectId = taskDto.ProjectId,
                Status = "Pending"
            };

            await taskRepository.CreateTaskAsync(task);

            return taskDto;
        }

        public async Task<List<TaskDtoV2>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await taskRepository.GetTasksByUserIdAsync(userId);
            return tasks.Select(x => new TaskDtoV2
            {
                Id = x.Id,
                ProjectName = x.Project.Name,
                ProjectId = x.ProjectId,
                Status = x.Status,
                UserName = x.User.Name,
                UserId = x.UserId,
                Description = x.Description,
                DueDate = x.DueDate,
                Title = x.Title
            }).ToList();
        }

        public async Task<List<TaskDtoV2>> GetTasksByProjectIdAsync(int projectId)
        {
            var tasks = await taskRepository.GetTasksByProjectIdAsync(projectId);
            return tasks.Select(x => new TaskDtoV2
            {
                Id = x.Id,
                ProjectName = x.Project.Name,
                ProjectId = x.ProjectId,
                Status = x.Status,
                UserName = x.User.Name,
                UserId = x.UserId,
                Description = x.Description,
                DueDate = x.DueDate,
                Title = x.Title
            }).ToList();
        }

        public async Task UpdateTaskStatusAsync(string taskId, string status)
        {
            var task = await taskRepository.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            task.Status = status;
            await taskRepository.UpdateTaskAsync(task);
        }

        public async Task<TaskDtoV2> GetTasksByIdAsync(string taskId)
        {
            var task = await taskRepository.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            var taskDto = new TaskDtoV2
            {
                ProjectName = task.Project.Name,
                UserName = task.User.Name,
                UserId = task.UserId,
                Status = task.Status,
                Description = task.Description,
                DueDate = task.DueDate,
                Title = task.Title,
                Id = task.Id,
                ProjectId = task.ProjectId
            };

            return taskDto;
        }

        public async Task<List<TaskDtoV2>> SearchTasksAsync(string keyword, string status, DateTime? dueDate, int pageNumber, int pageSize)
        {
            var query = taskRepository.GetAllTasksAsQueryable();

            if(!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Title.Contains(keyword) || x.Description.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (dueDate.HasValue)
            {
                query = query.Where(x => x.DueDate <= dueDate);
            }

            var tasks = query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .AsEnumerable()
                                   .ToList();

            return tasks.Select(x => new TaskDtoV2
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DueDate = x.DueDate,
                Status = x.Status,
                ProjectName = x.Project.Name,
                UserName = x.User.Name,
                ProjectId = x.Project.Id,
                UserId = x.User.Id
            }).ToList();
        }
    }
}
