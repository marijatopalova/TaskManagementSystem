using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectDto">The project details.</param>
        /// <remarks>Creates a new project.</remarks>
        /// <returns>Returns the created project.</returns>
        /// <response code="200">Returns the newly created project.</response>
        /// <response code="400">If the task is invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <remarks>Retrieves a list of all projects.</remarks>
        /// <returns>List of all projects.</returns>
        /// <response code="200">Returns a list of all projects.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ProjectDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProjectDto>>> GetAllProjects()
        {
            var projects = await projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        /// <summary>
        /// Retrieves a project by its ID.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <remarks>Retrieves a project by its ID.</remarks>
        /// <returns>The requested project.</returns>
        /// <response code="200">Returns the project details.</response>
        /// <response code="404">If project was not found.</response>
        [HttpGet("{projectId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int projectId)
        {
            var project = await projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound("Project was not found.");
            }
            return Ok(project);
        }

        /// <summary>
        /// Adds a user to a project.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <remarks>Adds a user to a project.</remarks>
        /// <returns>Status code with a message indicating success or failure.</returns>
        /// <response code="200">If the user was successfully added to the project.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost("{projectId}/users/{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserToProject(int projectId, int userId)
        {
            try
            {
                await projectService.AddUserToProjectAsync(projectId, userId);
                return Ok("User successfully added to the project.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Removes user from a project.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <remarks>Removes a user from a project.</remarks>
        /// <returns>Status code with a message indicating success or failure.</returns>
        /// <response code="200">If the user was successfully removed from the project.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpDelete("{projectId}/users/{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUserFromProject(int projectId, int userId)
        {
            try
            {
                await projectService.RemoveUserFromProjectAsync(projectId, userId);
                return Ok("User successfully removed from the project.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
