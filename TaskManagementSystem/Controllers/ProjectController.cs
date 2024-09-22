using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectDto">The project details.</param>
        /// <remarks>Creates a new project in the task management system.</remarks>
        /// <returns>Returns the created project.</returns>
        /// <response code="201">Returns the created project.</response>
        /// <response code="400">If the project details are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] ProjectDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProject = await projectService.CreateProjectAsync(projectDto);
            return Ok(createdProject);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var project = await projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPost("{projectId}/users/{userId}")]
        public async Task<IActionResult> AddUserToProject(int projectId, int userId)
        {
            try
            {
                await projectService.AddUserToProjectAsync(projectId, userId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{projectId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromProject(int projectId, int userId)
        {
            try
            {
                await projectService.RemoveUserFromProjectAsync(projectId, userId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
