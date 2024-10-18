using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BalanceApi.Controllers;
using BalanceApi.Services;
using BalanceApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Payment.Api.UnitTests
{
    public class BalanceControllerTests
    {
        private readonly Mock<IBalanceService> _mockBalanceService;
        private readonly BalanceController _controller;

        public BalanceControllerTests()
        {
            _mockBalanceService = new Mock<IBalanceService>();
            _controller = new BalanceController(_mockBalanceService.Object);
        }

        [Fact]
        public async Task UpdateBalance_UserExists_ShouldReturnOk()
        {
            // Arrange
            int userId = 1;
            decimal amount = 100m;
            _mockBalanceService.Setup(s => s.UpdateBalanceAsync(userId, amount)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateBalance(userId, amount);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Balance>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal("User Balance Details Successfully Updated", ((dynamic)okResult.Value).message);
        }

        [Fact]
        public async Task UpdateBalance_UserNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int userId = 1;
            decimal amount = 100m;
            _mockBalanceService.Setup(s => s.UpdateBalanceAsync(userId, amount)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateBalance(userId, amount);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Balance>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.Equal("User Not Available", ((dynamic)notFoundResult.Value).message);
        }
    }
}
