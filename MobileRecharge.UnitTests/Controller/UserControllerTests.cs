using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopUpAPI.Controllers;
using TopUpAPI.Models;
using TelecomProviderAPI.Application.Interfaces;
using MobileRecharge.Domain.Configuration;
using TopUpAPI.DataMapper;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MobileRecharge.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace MobileRecharge.UnitTests.Controller
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Appsettings _appSettings; // Use concrete class
        private readonly UserController _controller;

        public UserControllerTests()
        {
            // Mock the user service
            _userServiceMock = new Mock<IUserService>();

            // Create a concrete instance of Appsettings
            _appSettings = new Appsettings
            {
                UserMonthlyTopUpLimit = 100m // Set a test value
            };

            // Create the controller with the mocked dependencies
            _controller = new UserController(_userServiceMock.Object, _appSettings);
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
        public async Task GetUser_ExistingId_ReturnsOkResult_WithUserDto()
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
            var user = new User { Id = 1, Username = "NewUser", IsVerified = false, TotalTopUpLimit = 100m };
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
            var response = Assert.IsType<ResponseMessage>(okResult.Value); 
            Assert.Equal("User Details Successfully Updated", response.Message);
      
        }

        [Fact]
        public async Task UpdateUser_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _userServiceMock.Setup(service => service.UpdateUserAsync(1, true)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateUser(1, true);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<ResponseMessage>(notFoundResult.Value); 
            Assert.Equal("User Not Available", response.Message);
            
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
            var response = Assert.IsType< ResponseMessage> (okResult.Value); 
            Assert.Equal("User Successfully removed", response.Message);
        }
  
        [Fact]
        public async Task RemoveUser_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _userServiceMock.Setup(service => service.DeleteUserAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.RemoveUser(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = Assert.IsType<ResponseMessage>(notFoundResult.Value);
            Assert.Equal("User Not Available", response.Message);
        }
    }
}
