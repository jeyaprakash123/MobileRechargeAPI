using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelecomProviderAPI.Application.Interfaces;
using TelecomProviderAPI.Controllers;
using TopUpAPI.DataMapper;
using Xunit;

namespace MobileRecharge.UnitTests.Controller
{
    public class MobileRechargeControllerTests
    {
        private readonly Mock<IMobileRechargeService> _mockService;
        private readonly MobileRechargeController _controller;

        public MobileRechargeControllerTests()
        {
            _mockService = new Mock<IMobileRechargeService>();
            _controller = new MobileRechargeController(_mockService.Object);
        }

        [Fact]
        public async Task GetTopUpOptions_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var options = new List<TopUpOptionDto>
            {
                new TopUpOptionDto { Amount = 10 },
                new TopUpOptionDto { Amount = 20 }
            };

            _mockService.Setup(service => service.GetTopUpOptions())
                        .ReturnsAsync(options);

            // Act
            var result = await _controller.GetTopUpOptions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<TopUpOptionDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task TopUpBeneficiary_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            int userId = 1, beneficiaryId = 1;
            decimal amount = 10.0m;
            _mockService.Setup(service => service.TopUpBeneficiary(userId, beneficiaryId, amount))
                        .ReturnsAsync(true);

            // Act
            var result = await _controller.TopUpBeneficiary(userId, beneficiaryId, amount);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Top-up successful", okResult.Value);
        }

        [Fact]
        public async Task TopUpBeneficiary_ReturnsBadRequest_WhenAmountIsZeroOrNegative()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = -10.0m; // Invalid amount

            // Act
            var result = await _controller.TopUpBeneficiary(userId, beneficiaryId, amount);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Amount must be greater than zero", badRequestResult.Value);
        }
    }
}
