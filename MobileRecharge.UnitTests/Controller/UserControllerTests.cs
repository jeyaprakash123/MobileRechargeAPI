using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopUpAPI.Controllers;
using TopUpAPI.Models;
using TelecomProviderAPI.Application.Interfaces;
using TopUpAPI.DataMapper;

namespace MobileRecharge.UnitTests.Controller
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object, null);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<UserDto> { new UserDto { Id = 1, Username = "TestUser" } };
            _userServiceMock.Setup(service => service.GetUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnUsers = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.Single(returnUsers);
        }

        [Fact]
        public async Task GetUser_ExistingId_ReturnsOkResult_WithUser()
        {
            // Arrange
            var user = new UserDto { Id = 1, Username = "TestUser" };
            _userServiceMock.Setup(service => service.GetUser(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnUser = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal("TestUser", returnUser.Username);
        }

        [Fact]
        public async Task CreateUser_ValidUser_ReturnsCreatedResult()
        {
            // Arrange
            var user = new User { Id = 1, Username = "NewUser", IsVerified = false };
            _userServiceMock.Setup(service => service.CreateUserAsync(It.IsAny<User>()))
                            .ReturnsAsync(user);

            // Act
            var result = await _controller.CreateUser("NewUser", false);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(user.Id, createdResult.RouteValues["id"]);
        }

        [Fact]
        public async Task UpdateUser_ExistingId_ReturnsOkResult()
        {
            // Arrange
            _userServiceMock.Setup(service => service.UpdateUserAsync(1, true)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateUser(1, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("User Details Successfully Updated", ((dynamic)okResult.Value).message);
        }

        [Fact]
        public async Task RemoveUser_ExistingId_ReturnsOkResult()
        {
            // Arrange
            _userServiceMock.Setup(service => service.DeleteUserAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("User Successfully removed", ((dynamic)okResult.Value).message);
        }
    }
}
