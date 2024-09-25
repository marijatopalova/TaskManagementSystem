using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.Entities;
using TaskManagementSystem.Repositories;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IProjectRepository> _mockProjectRepository;
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _userService = new UserService(_mockUserRepository.Object, _mockProjectRepository.Object);
        }

        [Test]
        public async Task CreateUserAsync_ShouldCreateUser_WhenValidUserDtoIsProvided()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Test",
                Email = "test@example.com"
            };

            _mockUserRepository
                .Setup(x => x.CreateUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask); 

            // Act
            var result = await _userService.CreateUserAsync(userDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo(userDto.Name));
                Assert.That(result.Email, Is.EqualTo(userDto.Email));
            });
        }

        [Test]
        public async Task GetAllUsersAsync_ShouldReturnListOfUserDtos_WhenUsersExist()
        {
            // Arrange
            var users = new List<User>
            {
                new() { Id = 1, Name = "Test User One", Email = "test1@example.com" },
                new() { Id = 2, Name = "Test User Two", Email = "test2@example.com" }
            };

            _mockUserRepository
                .Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(users); 

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Name, Is.EqualTo(users[0].Name));
                Assert.That(result[0].Email, Is.EqualTo(users[0].Email));
                Assert.That(result[0].Id, Is.EqualTo(users[0].Id));
                Assert.That(result[1].Name, Is.EqualTo(users[1].Name));
                Assert.That(result[1].Email, Is.EqualTo(users[1].Email));
                Assert.That(result[1].Id, Is.EqualTo(users[1].Id));
            });
        }

        [Test]
        public async Task GetUserByIdAsync_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User
            {
                Id = userId,
                Name = "Test User",
                Email = "test@example.com"
            };

            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(user); 

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(userId));
                Assert.That(result.Name, Is.EqualTo("Test User"));
                Assert.That(result.Email, Is.EqualTo("test@example.com"));
            });
        }

        [Test]
        public void GetUserByIdAsync_ShouldThrowArgumentException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;

            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync((User)null); 

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _userService.GetUserByIdAsync(userId));

            Assert.That(exception.Message, Is.EqualTo("User not found"));
            _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task GetUsersByProjectIdAsync_ShouldReturnUsers_WhenProjectExists()
        {
            // Arrange
            var projectId = 1;
            var project = new Project
            {
                Id = projectId,
                Name = "Test Project",
                Users =
                [
                    new User { Id = 1, Name = "User One", Email = "user1@example.com" },
                    new User { Id = 2, Name = "User Two", Email = "user2@example.com" }
                ]
            };

            _mockProjectRepository
                .Setup(repo => repo.GetProjectByIdAsync(projectId))
                .ReturnsAsync(project); 

            // Act
            var result = await _userService.GetUsersByProjectIdAsync(projectId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));

            Assert.Multiple(() =>
            {
                Assert.That(result[0].Id, Is.EqualTo(1));
                Assert.That(result[0].Name, Is.EqualTo("User One"));
                Assert.That(result[0].Email, Is.EqualTo("user1@example.com"));

                Assert.That(result[1].Id, Is.EqualTo(2));
                Assert.That(result[1].Name, Is.EqualTo("User Two"));
                Assert.That(result[1].Email, Is.EqualTo("user2@example.com"));
            });
        }

        [Test]
        public void GetUsersByProjectIdAsync_ShouldThrowArgumentException_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 1;

            _mockProjectRepository.Setup(x => x.GetProjectByIdAsync(projectId))
                .ReturnsAsync((Project)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => 
                await _userService.GetUsersByProjectIdAsync(projectId));

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Project not found"));

            _mockProjectRepository.Verify(x => x.GetProjectByIdAsync(projectId), Times.Once());
        } 
    }
}
