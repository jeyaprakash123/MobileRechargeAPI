using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopUpAPI.Controllers;
using TelecomProviderAPI.Application.Interfaces;
using TopUpAPI.Models;
using Xunit;
using TopUpAPI.DataMapper;

namespace MobileRecharge.UnitTests.Controller
{
    public class BeneficiaryControllerTests
    {
        private readonly Mock<IBeneficiaryService> _mockService;
        private readonly BeneficiaryController _controller;

        public BeneficiaryControllerTests()
        {
            _mockService = new Mock<IBeneficiaryService>();
            _controller = new BeneficiaryController(_mockService.Object);
        }

        [Fact]
        public async Task GetBeneficiaries_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var userId = 1;
            var beneficiaries = new List<BeneficiaryDto>
        {
            new BeneficiaryDto { Id = 1, Nickname = "John Doe" }
        };

            _mockService.Setup(service => service.GetBeneficiaries(userId))
                        .ReturnsAsync(beneficiaries);

            // Act
            var result = await _controller.GetBeneficiaries(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<BeneficiaryDto>>(okResult.Value);
            Assert.Equal(1, returnValue.Count);
        }

        [Fact]
        public async Task AddBeneficiary_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var userId = 1;
            var nickname = "John Doe";

            _mockService.Setup(service => service.AddBeneficiary(userId, nickname))
                        .ReturnsAsync(true);

            // Act
            var result = await _controller.AddBeneficiary(userId, nickname);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Beneficiary added successfully", okResult.Value);
        }

        [Fact]
        public async Task RemoveBeneficiary_ReturnsNotFound_WhenBeneficiaryDoesNotExist()
        {
            // Arrange
            var beneficiaryId = 1;
            _mockService.Setup(service => service.DeleteBeneficiary(beneficiaryId))
                        .ReturnsAsync(false);

            // Act
            var result = await _controller.RemoveBeneficiary(beneficiaryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Beneficiay Not Available", ((dynamic)notFoundResult.Value).message);
        }
        [Fact]
        public async Task AddBeneficiary_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var userId = 1;
            var nickname = ""; // Invalid nickname

            _mockService.Setup(service => service.AddBeneficiary(userId, nickname))
                        .ThrowsAsync(new ArgumentException("Nickname cannot be empty"));

            // Act
            var result = await _controller.AddBeneficiary(userId, nickname);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Nickname cannot be empty", badRequestResult.Value);
        }
        [Fact]
        public async Task GetBeneficiaries_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var userId = 1;
            _mockService.Setup(service => service.GetBeneficiaries(userId))
                        .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetBeneficiaries(userId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Unexpected error", statusCodeResult.Value);
        }
        [Fact]
        public async Task RemoveBeneficiary_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var beneficiaryId = 1;
            _mockService.Setup(service => service.DeleteBeneficiary(beneficiaryId))
                        .ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveBeneficiary(beneficiaryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Beneficiary removed Successfully", ((dynamic)okResult.Value).message);
        }

    }
}
