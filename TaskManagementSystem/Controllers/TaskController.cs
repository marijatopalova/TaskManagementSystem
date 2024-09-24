using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("api/task")]
    [ApiController]
    public class TaskController(ITaskService taskService) : ControllerBase
    {
        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="taskDto">The details of the task to create.</param>
        /// <returns>The created task.</returns>
        /// <response code="200">Returns the newly created task.</response>
        /// <response code="400">If the task is invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTask = await taskService.CreateTaskAsync(taskDto);
                return Ok(createdTask);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves task by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Returns the task.</returns>
        /// <response code="200">Returns the task.</response>
        /// <response code="404">If the task is not found.</response>
        [HttpGet("user/{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> GetTasksByUser(int userId)
        {
            var tasks = await taskService.GetTasksByUserIdAsync(userId);
            if (tasks == null || tasks.Count == 0)
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        /// <summary>
        /// Retrieves task by project ID.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <returns>Returns the task.</returns>
        /// <response code="200">Returns the task.</response>
        /// <response code="404">If the task is not found.</response>
        [HttpGet("project/{projectId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> GetTasksByProject(int projectId)
        {
            var tasks = await taskService.GetTasksByProjectIdAsync(projectId);
            if (tasks == null || tasks.Count == 0)
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="taskId">The ID of the task to update.</param>
        /// <param name="status">The updated task status.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Task updated successfully.</response>
        /// <response code="404">If the task is not found.</response>
        [HttpPatch("{taskId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTaskStatus(string taskId, [FromBody] string status)
        {
            try
            {
                await taskService.UpdateTaskStatusAsync(taskId, status);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve.</param>
        /// <returns>The task with the specified ID.</returns>
        /// <response code="200">Returns the task.</response>
        /// <response code="404">If the task is not found.</response>
        [HttpGet("{taskId}")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> GetTaskById(string taskId)
        {
            var task = await taskService.GetTasksByIdAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }
    }
}
