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
using TaskManagementSystem.Services.V1;

namespace TaskManagementSystem.Tests.Services
{
    [TestFixture]
    public class ProjectServiceTests
    {
        private Mock<IProjectRepository> _mockProjectRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private ProjectService _projectService;

        [SetUp]
        public void SetUp()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _projectService = new ProjectService(_mockProjectRepository.Object, _mockUserRepository.Object);
        }

        [Test]
        public async Task CreateProjectAsync_ShouldCreateProjectAndReturnProjectDto()
        {
            // Arrange
            var projectDto = new ProjectDto
            {
                Name = "New Project",
                Description = "This is a test project"
            };

            // Act
            var result = await _projectService.CreateProjectAsync(projectDto);

            // Assert
            _mockProjectRepository.Verify(m => m.CreateProjectAsync(It.Is<Project>(p =>
                p.Name == projectDto.Name &&
                p.Description == projectDto.Description)), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo(projectDto.Name));
                Assert.That(result.Description, Is.EqualTo(projectDto.Description));
            });
        }

        [Test]
        public async Task GetAllProjectsAsync_ShouldReturnListOfProjectDtos()
        {
            // Arrange
            var projects = new List<Project>
            {
                new() {
                    Id = 1,
                    Name = "Project 1",
                    Description = "Description 1",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30)
                },
                new() {
                    Id = 2,
                    Name = "Project 2",
                    Description = "Description 2",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(60)
                }
            };

            _mockProjectRepository.Setup(m => m.GetAllProjectsAsync()).ReturnsAsync(projects);

            // Act
            var result = await _projectService.GetAllProjectsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Name, Is.EqualTo("Project 1"));
                Assert.That(result[0].Description, Is.EqualTo("Description 1"));
                Assert.That(result[0].Id, Is.EqualTo(1));
                Assert.That(result[1].Name, Is.EqualTo("Project 2"));
                Assert.That(result[1].Description, Is.EqualTo("Description 2"));
                Assert.That(result[1].Id, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetProjectByIdAsync_ExistingProjectId_ShouldReturnProjectDto()
        {
            // Arrange
            var projectId = 1;
            var project = new Project
            {
                Id = projectId,
                Name = "Test",
                Description = "Test"
            };

            _mockProjectRepository.Setup(x => x.GetProjectByIdAsync(projectId)).ReturnsAsync(project);

            // Act
            var result = await _projectService.GetProjectByIdAsync(projectId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(project.Id));
                Assert.That(result.Name, Is.EqualTo(project.Name));
                Assert.That(result.Description, Is.EqualTo(project.Description));
            });
        }

        [Test]
        public void GetProjectByIdAsync_NonExistingProjectId_ShouldThrowArgumentException()
        {
            // Arrange
            var projectId = 999; // A non-existing ID
            _mockProjectRepository.Setup(x => x.GetProjectByIdAsync(projectId)).ReturnsAsync((Project)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _projectService.GetProjectByIdAsync(projectId));
            Assert.That(exception.Message, Is.EqualTo("Project not found"));
        }

        [Test]
        public async Task AddUserToProjectAsync_ValidInput_ShouldAddUserToProject()
        {
            // Arrange
            var projectId = 1;
            var userId = 2;
            var project = new Project
            {
                Id = projectId,
                Name = "Project 1",
                Users = [] 
            };

            var user = new User
            {
                Id = userId,
                Name = "User 1"
            };

            _mockProjectRepository.Setup(m => m.GetProjectByIdAsync(projectId)).ReturnsAsync(project);
            _mockUserRepository.Setup(m => m.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            await _projectService.AddUserToProjectAsync(projectId, userId);

            // Assert
            Assert.That(project.Users.Any(u => u.Id == userId), Is.True);
        }

        [Test]
        public async Task AddUserToProjectAsync_ProjectNotFound_ShouldThrowArgumentException()
        {
            // Arrange
            var projectId = 1;
            var userId = 2;

            _mockProjectRepository.Setup(m => m.GetProjectByIdAsync(projectId)).ReturnsAsync((Project)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _projectService.AddUserToProjectAsync(projectId, userId));
            Assert.That(exception.Message, Is.EqualTo("Project not found"));
        }

        [Test]
        public async Task AddUserToProjectAsync_UserNotFound_ShouldThrowArgumentException()
        {
            // Arrange
            var projectId = 1;
            var userId = 2;
            var project = new Project
            {
                Id = projectId,
                Users = []
            };

            _mockProjectRepository.Setup(m => m.GetProjectByIdAsync(projectId)).ReturnsAsync(project);
            _mockUserRepository.Setup(m => m.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _projectService.AddUserToProjectAsync(projectId, userId));
            Assert.That(exception.Message, Is.EqualTo("User not found"));
            _mockProjectRepository.Verify(m => m.GetProjectByIdAsync(projectId), Times.Once);
            _mockUserRepository.Verify(m => m.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task AddUserToProjectAsync_UserAlreadyInProject_ShouldThrowArgumentException()
        {
            // Arrange
            var projectId = 1;
            var userId = 2;
            var project = new Project
            {
                Id = projectId,
                Users =
                [
                    new() { Id = userId } 
                ]
            };

            var user = new User
            {
                Id = userId,
                Name = "User 1"
            };

            _mockProjectRepository.Setup(m => m.GetProjectByIdAsync(projectId)).ReturnsAsync(project);
            _mockUserRepository.Setup(m => m.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _projectService.AddUserToProjectAsync(projectId, userId));
            Assert.That(exception.Message, Is.EqualTo("User is already part of the project"));
            _mockProjectRepository.Verify(m => m.UpdateProjectAsync(It.IsAny<Project>()), Times.Never); 
        }

        [Test]
        public async Task RemoveUserFromProjectAsync_ValidInput_ShouldRemoveUserFromProject()
        {
            // Arrange
            var projectId = 1;
            var userId = 2;
            var project = new Project
            {
                Id = projectId,
                Name = "Project 1",
                Users =
                [
                    new() { Id = userId } 
                ]
            };

            _mockProjectRepository.Setup(m => m.GetProjectByIdAsync(projectId)).ReturnsAsync(project);

            // Act
            await _projectService.RemoveUserFromProjectAsync(projectId, userId);

            // Assert
            Assert.That(project.Users.Any(u => u.Id == userId), Is.False);
        }

        [Test]
        public async Task RemoveUserFromProjectAsync_ProjectNotFound_ShouldThrowArgumentException()
        {
            // Arrange
            var projectId = 1;
            var userId = 2;

            _mockProjectRepository.Setup(m => m.GetProjectByIdAsync(projectId)).ReturnsAsync((Project)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _projectService.RemoveUserFromProjectAsync(projectId, userId));
            Assert.That(exception.Message, Is.EqualTo("Project not found"));
        }

        [Test]
        public async Task RemoveUserFromProjectAsync_UserNotPartOfProject_ShouldThrowArgumentException()
        {
            // Arrange
            var projectId = 1;
            var userId = 2;
            var project = new Project
            {
                Id = projectId,
                Users = []
            };

            _mockProjectRepository.Setup(m => m.GetProjectByIdAsync(projectId)).ReturnsAsync(project);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _projectService.RemoveUserFromProjectAsync(projectId, userId));
            Assert.That(exception.Message, Is.EqualTo("User is not part of the project"));
            _mockProjectRepository.Verify(m => m.UpdateProjectAsync(It.IsAny<Project>()), Times.Never); 
        }
    }
}
