using AutoMapper;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Repositories;

namespace TaskManagementSystem.Services
{
    public class TaskService(ITaskRepository taskRepository, 
        IUserRepository userRepository, 
        IProjectRepository projectRepository) : ITaskService
    {
        public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto)
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

            if(project.Users.Any(x => x.Id == taskDto.UserId))
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

        public async Task<List<TaskDto>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await taskRepository.GetTasksByUserIdAsync(userId);
            return tasks.Select(x => new TaskDto
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

        public async Task<List<TaskDto>> GetTasksByProjectIdAsync(int projectId)
        {
            var tasks = await taskRepository.GetTasksByProjectIdAsync(projectId);
            return tasks.Select(x => new TaskDto
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

        public async Task<TaskDto> GetTasksByIdAsync(string taskId)
        {
            var task = await taskRepository.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            var taskDto = new TaskDto
            {
                ProjectName = task.Project.Name,
                UserName = task.User.Name,
                UserId = task.UserId,
                Status = task.Status,
                Description = task.Description,
                DueDate = task.DueDate,
                Title = task.Title
            };

            return taskDto;
        }
    }
}
