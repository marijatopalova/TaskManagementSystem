using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Entities;

namespace TaskManagementSystem.Repositories
{
    public class TaskRepository(TaskManagementDbContext dbContext) : ITaskRepository
    {
        public async Task CreateTaskAsync(TaskItem task)
        {
            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await dbContext.Tasks.FindAsync(id);

            if (task != null)
            {
                dbContext.Tasks.Remove(task);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await dbContext.Tasks
                .Include(x => x.User)
                .Include(x => x.Project)
                .ToListAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(string id)
        {
            return await dbContext.Tasks
                                    .Include(x => x.User)
                                    .Include(x => x.Project)
                                    .Where(x => x.Id.ToString() == id)
                                    .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId)
        {
            return await dbContext.Tasks
                                    .Include(x => x.User)
                                    .Include(x => x.Project)
                                    .Where(x => projectId == x.ProjectId)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId)
        {
            return await dbContext.Tasks
                                    .Include(x => x.User)
                                    .Include(x => x.Project)
                                    .Where(x => x.UserId == userId)
                                    .ToListAsync();
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            dbContext.Tasks.Update(task);
            await dbContext.SaveChangesAsync();
        }
    }
}
