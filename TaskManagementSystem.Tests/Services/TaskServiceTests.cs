using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Repositories;
using TaskManagementSystem.Repositories;
using TaskManagementSystem.Services;
using TaskManagementSystem.Services.V1;

namespace TaskManagementSystem.Tests.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _mockTaskRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IProjectRepository> _mockProjectRepository;
        private ITaskService _taskService;

        [SetUp]
        public void Setup()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _taskService = new TaskService(_mockTaskRepository.Object, _mockUserRepository.Object, _mockProjectRepository.Object);
        }

        [Test]
        public async Task CreateTaskAsync_ShouldCreateTask_WhenUserAndProjectAreValid()
        {
            // Arrange
            var taskDto = new TaskDto
            {
                Title = "Test Task",
                Description = "Test Task Description",
                DueDate = DateTime.Now.AddDays(7),
                UserId = 1,
                ProjectId = 1
            };

            var user = new User { Id = taskDto.UserId, Name = "Test User" };
            var project = new Project
            {
                Id = taskDto.ProjectId,
                Name = "Test Project",
                Users = [new User { Id = taskDto.UserId }]
            };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(taskDto.UserId)).ReturnsAsync(user);
            _mockProjectRepository.Setup(repo => repo.GetProjectByIdAsync(taskDto.ProjectId)).ReturnsAsync(project);

            // Act
            var result = await _taskService.CreateTaskAsync(taskDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Title, Is.EqualTo(taskDto.Title));
                Assert.That(result.Description, Is.EqualTo(taskDto.Description));
                Assert.That(result.UserId, Is.EqualTo(taskDto.UserId));
                Assert.That(result.ProjectId, Is.EqualTo(taskDto.ProjectId));
            });
        }

        [Test]
        public void CreateTaskAsync_ShouldThrowArgumentException_WhenUserDoesNotExist()
        {
            // Arrange
            var taskDto = new TaskDto { UserId = 1, ProjectId = 1 };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(taskDto.UserId))
                .ReturnsAsync((User)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _taskService.CreateTaskAsync(taskDto));

            Assert.That(exception.Message, Is.EqualTo("User not found"));

            _mockTaskRepository.Verify(repo => repo.CreateTaskAsync(It.IsAny<TaskItem>()), Times.Never);
        }

        [Test]
        public void CreateTaskAsync_ShouldThrowArgumentException_WhenProjectDoesNotExist()
        {
            // Arrange
            var taskDto = new TaskDto { UserId = 1, ProjectId = 1 };
            var user = new User { Id = taskDto.UserId };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(taskDto.UserId))
                .ReturnsAsync(user);
            _mockProjectRepository.Setup(repo => repo.GetProjectByIdAsync(taskDto.ProjectId))
                .ReturnsAsync((Project)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _taskService.CreateTaskAsync(taskDto));

            Assert.That(exception.Message, Is.EqualTo("Project not found"));

            _mockTaskRepository.Verify(repo => repo.CreateTaskAsync(It.IsAny<TaskItem>()), Times.Never);
        }

        [Test]
        public void CreateTaskAsync_ShouldThrowArgumentException_WhenUserIsNotPartOfProject()
        {
            // Arrange
            var taskDto = new TaskDto { UserId = 1, ProjectId = 1 };
            var user = new User { Id = taskDto.UserId };
            var project = new Project
            {
                Id = taskDto.ProjectId,
                Users = [new User { Id = 2 }]
            };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(taskDto.UserId)).ReturnsAsync(user);
            _mockProjectRepository.Setup(repo => repo.GetProjectByIdAsync(taskDto.ProjectId)).ReturnsAsync(project);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _taskService.CreateTaskAsync(taskDto));

            Assert.That(exception.Message, Is.EqualTo("User is not part of the project"));

            _mockTaskRepository.Verify(repo => repo.CreateTaskAsync(It.IsAny<TaskItem>()), Times.Never);
        }

        [Test]
        public async Task GetTasksByProjectIdAsync_ShouldReturnTasks_WhenTasksExist()
        {
            // Arrange
            int projectId = 1;
            var tasks = new List<TaskItem>
            {
                new() {
                    Id = 1,
                    Project = new Project { Id = projectId, Name = "Project 1" },
                    ProjectId = projectId,
                    Status = "Pending",
                    User = new User { Id = 1, Name = "User 1" },
                    UserId = 1,
                    Description = "Test Task 1",
                    DueDate = System.DateTime.Now.AddDays(5),
                    Title = "Task 1"
                }
            };

            _mockTaskRepository.Setup(repo => repo.GetTasksByProjectIdAsync(projectId))
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetTasksByProjectIdAsync(projectId);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(result.First().ProjectName, Is.EqualTo("Project 1"));
                Assert.That(result.First().ProjectId, Is.EqualTo(projectId));
                Assert.That(result.First().Status, Is.EqualTo("Pending"));
            });
        }

        [Test]
        public async Task GetTasksByProjectIdAsync_ShouldReturnEmptyList_WhenNoTasksExist()
        {
            // Arrange
            int projectId = 1;
            _mockTaskRepository.Setup(repo => repo.GetTasksByProjectIdAsync(projectId))
                .ReturnsAsync([]);

            // Act
            var result = await _taskService.GetTasksByProjectIdAsync(projectId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task UpdateTaskStatusAsync_ShouldThrowException_WhenTaskNotFound()
        {
            // Arrange
            string taskId = "1";
            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(taskId))
                .ReturnsAsync((TaskItem)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _taskService.UpdateTaskStatusAsync(taskId, "Completed"));
            Assert.That(ex.Message, Is.EqualTo("Task not found"));
            _mockTaskRepository.Verify(repo => repo.GetTaskByIdAsync(taskId), Times.Once);
            _mockTaskRepository.Verify(repo => repo.UpdateTaskAsync(It.IsAny<TaskItem>()), Times.Never);
        }

        [Test]
        public async Task UpdateTaskStatusAsync_ShouldUpdateStatus_WhenTaskIsFound()
        {
            // Arrange
            int taskId = 1;
            var task = new TaskItem { Id = taskId, Status = "Pending" };

            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(taskId.ToString()))
                .ReturnsAsync(task);

            // Act
            await _taskService.UpdateTaskStatusAsync(taskId.ToString(), "Completed");

            // Assert
            Assert.That(task.Status, Is.EqualTo("Completed"));
            _mockTaskRepository.Verify(repo => repo.GetTaskByIdAsync(taskId.ToString()), Times.Once);
            _mockTaskRepository.Verify(repo => repo.UpdateTaskAsync(task), Times.Once);
        }

        [Test]
        public async Task GetTasksByIdAsync_ShouldThrowException_WhenTaskNotFound()
        {
            // Arrange
            string taskId = "1";
            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(taskId))
                .ReturnsAsync((TaskItem)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _taskService.GetTasksByIdAsync(taskId));
            Assert.AreEqual("Task not found", ex.Message);
            _mockTaskRepository.Verify(repo => repo.GetTaskByIdAsync(taskId), Times.Once);
        }

        [Test]
        public async Task GetTasksByIdAsync_ShouldReturnTaskDto_WhenTaskIsFound()
        {
            // Arrange
            int taskId = 1;
            var task = new TaskItem
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = System.DateTime.Now.AddDays(5),
                Status = "Pending",
                Project = new Project { Id = 1, Name = "Project 1" },
                User = new User { Id = 1, Name = "User 1" }
            };

            _mockTaskRepository.Setup(repo => repo.GetTaskByIdAsync(taskId.ToString()))
                .ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTasksByIdAsync(taskId.ToString());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Title, Is.EqualTo("Test Task"));
                Assert.That(result.Status, Is.EqualTo("Pending"));
                Assert.That(result.UserName, Is.EqualTo("User 1"));
                Assert.That(result.ProjectName, Is.EqualTo("Project 1"));
            });
        }
    }
}
