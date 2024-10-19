using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BalanceApi.Controllers;
using BalanceApi.Services;
using BalanceApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Payment.Api.Models;

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
        public async Task MakePayment_UserExists_ShouldReturnOk()
        {
            // Arrange
            int userId = 1;
            var request = new PaymentRequest { TotalAmount = 100m };
            _mockBalanceService.Setup(s => s.UpdateBalanceAsync(userId, request.TotalAmount)).ReturnsAsync(true);

            // Act
            var result = await _controller.MakePayment(userId, request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Balance>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<ResponseMessage>(okResult.Value);
            Assert.Equal("User Balance Details Successfully Updated", response.Message);
        }

        [Fact]
        public async Task MakePayment_UserNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int userId = 1;
            var request = new PaymentRequest { TotalAmount = 100m };
            _mockBalanceService.Setup(s => s.UpdateBalanceAsync(userId, request.TotalAmount)).ReturnsAsync(false);

            // Act
            var result = await _controller.MakePayment(userId, request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Balance>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var response = Assert.IsType<ResponseMessage>(notFoundResult.Value);
            Assert.Equal("User Not Available", response.Message);
        }

        [Fact]
        public async Task MakePayment_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            int userId = 1;
            PaymentRequest request = null; // Invalid request

            // Act
            var result = await _controller.MakePayment(userId, request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Balance>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ResponseMessage>(badRequestResult.Value);
            Assert.Equal("Invalid payment request", response.Message);
        }

    }
}
