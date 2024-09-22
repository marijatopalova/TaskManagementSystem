using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repositories
{
    public class ProjectRepository(TaskManagementDbContext dbContext) : IProjectRepository
    {
        public async Task CreateProjectAsync(Project project)
        {
            dbContext.Add(project);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await dbContext.Projects.FindAsync(id);

            if (project != null)
            {
                dbContext.Projects.Remove(project);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await dbContext.Projects
                                    .Include(x => x.Users)
                                    .Include(x => x.Tasks)
                                    .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await dbContext.Projects
                                    .Include(x => x.Users)
                                    .Include(x => x.Tasks)
                                    .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            dbContext.Projects.Update(project);
            await dbContext.SaveChangesAsync();
        }
    }
}
