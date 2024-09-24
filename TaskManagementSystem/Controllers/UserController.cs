using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">The user details.</param>
        /// <remarks>Creates a new user.</remarks>
        /// <returns>The created user.</returns>
        /// <response code="200">Returns the newly created user.</response>
        /// <response code="400">If the user details are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await userService.CreateUserAsync(userDto);
            return Ok(createdUser);
        }

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <remarks>Retrieves a list of all users.</remarks>
        /// <returns>List of all users.</returns>
        /// <response code="200">Returns the list of all users.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user by its ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Returns the user.</returns>
        /// <remarks>Retrieves a user by its ID.</remarks>
        /// <response code="200">Returns the user details.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpGet("{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById(int userId)
        {
            var user = await userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User was not found.");
            }
            return Ok(user);
        }

        /// <summary>
        /// Retrieves a list of users by projectId.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <remarks>Retrieves a list of users by project ID</remarks>
        /// <returns>List of users.</returns>
        /// <response code="200">Returns a list of users found in the project.</response>
        /// <response code="404">If no users were found in the project.</response>
        [HttpGet("project/{projectId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersByProject(int projectId)
        {
            var users = await userService.GetUsersByProjectIdAsync(projectId);
            if (users == null || users.Count == 0)
            {
                return NotFound("No users were found in this project.");
            }
            return Ok(users);
        }
    }
}
