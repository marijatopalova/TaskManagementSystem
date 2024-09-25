using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Tests.Controllers
{
    [TestFixture]
    public class ProjectControllerTests
    {
        private Mock<IProjectService> _mockProjectService;
        private ProjectController _projectController;

        [SetUp]
        public void Setup()
        {
            _mockProjectService = new Mock<IProjectService>();
            _projectController = new ProjectController(_mockProjectService.Object);
        }

        [Test]
        public async Task CreateProject_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _projectController.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _projectController.CreateProject(new ProjectDto());

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateProject_ShouldReturnOk_WhenProjectIsCreated()
        {
            // Arrange
            var projectDto = new ProjectDto { Name = "New Project" };
            _mockProjectService.Setup(service => service.CreateProjectAsync(projectDto))
                .ReturnsAsync(projectDto);

            // Act
            var result = await _projectController.CreateProject(projectDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(projectDto));
            });
        }

        [Test]
        public async Task GetAllProjects_ShouldReturnOkWithProjectList()
        {
            // Arrange
            var projects = new List<ProjectDto>
            {
                new() { Name = "Project 1" },
                new() { Name = "Project 2" }
            };

            _mockProjectService.Setup(service => service.GetAllProjectsAsync())
                .ReturnsAsync(projects);

            // Act
            var result = await _projectController.GetAllProjects();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(projects));
            });
        }

        [Test]
        public async Task GetProjectById_ShouldReturnNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            int projectId = 1;
            _mockProjectService.Setup(service => service.GetProjectByIdAsync(projectId))
                .ReturnsAsync((ProjectDto)null);

            // Act
            var result = await _projectController.GetProjectById(projectId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        [Test]
        public async Task GetProjectById_ShouldReturnOk_WhenProjectIsFound()
        {
            // Arrange
            int projectId = 1;
            var project = new ProjectDto { Name = "Project 1" };

            _mockProjectService.Setup(service => service.GetProjectByIdAsync(projectId))
                .ReturnsAsync(project);

            // Act
            var result = await _projectController.GetProjectById(projectId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(project));
            });
        }

        [Test]
        public async Task AddUserToProject_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
        {
            // Arrange
            int projectId = 1;
            int userId = 1;
            _mockProjectService.Setup(service => service.AddUserToProjectAsync(projectId, userId))
                .ThrowsAsync(new ArgumentException("User not found"));

            // Act
            var result = await _projectController.AddUserToProject(projectId, userId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
                Assert.That(badRequestResult.Value, Is.EqualTo("User not found"));
            });
        }

        [Test]
        public async Task AddUserToProject_ShouldReturnOk_WhenUserIsAddedSuccessfully()
        {
            // Arrange
            int projectId = 1;
            int userId = 1;
            _mockProjectService.Setup(service => service.AddUserToProjectAsync(projectId, userId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _projectController.AddUserToProject(projectId, userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo("User successfully added to the project."));
            });
        }

        [Test]
        public async Task RemoveUserFromProject_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
        {
            // Arrange
            int projectId = 1;
            int userId = 1;
            _mockProjectService.Setup(service => service.RemoveUserFromProjectAsync(projectId, userId))
                .ThrowsAsync(new ArgumentException("User not found"));

            // Act
            var result = await _projectController.RemoveUserFromProject(projectId, userId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
                Assert.That(badRequestResult.Value, Is.EqualTo("User not found"));
            });
        }

        [Test]
        public async Task RemoveUserFromProject_ShouldReturnOk_WhenUserIsRemovedSuccessfully()
        {
            // Arrange
            int projectId = 1;
            int userId = 1;
            _mockProjectService.Setup(service => service.RemoveUserFromProjectAsync(projectId, userId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _projectController.RemoveUserFromProject(projectId, userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo("User successfully removed from the project."));
            });
        }
    }
}
