using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TelecomProviderAPI.Core.IRepository;
using TopUpAPI.Models;
using MobileRecharge.Domain.UnitOfWork;
using TopUpAPI.Services;
using Xunit;
using TopUpAPI.DataMapper;

namespace MobileRecharge.UnitTests.Service
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory; // Mock for HttpClientFactory
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>(); // Initialize the HttpClientFactory mock

            _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);
            _mockUserRepository.Setup(u => u.GetUsersAsync()).ReturnsAsync(new List<User>());
            _userService = new UserService(_mockUnitOfWork.Object, null, _mockMapper.Object, _mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnListOfUserDtos()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "Alice" },
                new User { Id = 2, Username = "Bob" }
            };
            var userDtos = new List<UserDto>
            {
                new UserDto { Id = 1, Username = "Alice" },
                new UserDto { Id = 2, Username = "Bob" }
            };

            _mockUserRepository.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(users);
            _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.IsType<List<UserDto>>(result);
        }

        [Fact]
        public async Task GetUser_ValidId_ShouldReturnUserDto()
        {
            // Arrange
            int userId = 1;
            var user = new User { Id = userId, Username = "Alice" };
            var userDto = new UserDto { Id = userId, Username = "Alice" };

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetUser(userId);

            // Assert
            Assert.Equal(userId, result.Value.Id);
            Assert.IsType<UserDto>(result.Value);
        }

        [Fact]
        public async Task GetUser_InvalidId_ShouldReturnNull()
        {
            // Arrange
            int userId = 1;

            _mockUserRepository.Setup(repo => repo.GetUser(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUser(userId);

            // Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task CreateUser_ValidUser_ShouldReturnUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "Alice" };

            // Mock the balance retrieval
            _mockUnitOfWork.Setup(u => u.MobileRechargeRepository.GetUserBalance(user.Id)).ReturnsAsync(100m);

            _mockUserRepository.Setup(repo => repo.CreateUserAsync(user)).ReturnsAsync(user);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _userService.CreateUserAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(100m, result.AvailableBalance); // Ensure the balance is also set
        }


        [Fact]
        public async Task UpdateUser_ValidId_ShouldReturnTrue()
        {
            // Arrange
            int userId = 1;
            bool isVerified = true;
            _mockUserRepository.Setup(repo => repo.UpdateUserAsync(userId, isVerified)).ReturnsAsync(true);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _userService.UpdateUserAsync(userId, isVerified);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUser_InvalidId_ShouldReturnFalse()
        {
            // Arrange
            int userId = 1;
            bool isVerified = true;
            _mockUserRepository.Setup(repo => repo.UpdateUserAsync(userId, isVerified)).ReturnsAsync(false);

            // Act
            var result = await _userService.UpdateUserAsync(userId, isVerified);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUser_ValidId_ShouldReturnTrue()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(userId)).ReturnsAsync(true);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUser_InvalidId_ShouldReturnFalse()
        {
            // Arrange
            int userId = 1;
            _mockUserRepository.Setup(repo => repo.DeleteUserAsync(userId)).ReturnsAsync(false);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result);
        }
    }
}
