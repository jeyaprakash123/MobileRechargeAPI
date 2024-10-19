using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MobileRecharge.Domain.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TelecomProviderAPI.Core.IRepository;
using TelecomProviderAPI.Services;
using TopUpAPI.DataMapper;
using TopUpAPI.Models;
using Xunit;

namespace MobileRecharge.UnitTests.Service
{
    public class MobileRechargeServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMobileRechargeRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly MobileRechargeService _service;

        public MobileRechargeServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IMobileRechargeRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            // Mock HttpClient
            var mockHttpClient = new HttpClient();
            _mockHttpClientFactory.Setup(f => f.CreateClient("PaymentAPI")).Returns(mockHttpClient);

            _mockUnitOfWork.Setup(u => u.MobileRechargeRepository).Returns(_mockRepository.Object);
            _service = new MobileRechargeService(_mockUnitOfWork.Object, _mockHttpClientFactory.Object, null, _mockMapper.Object);
        }

        [Fact]
        public async Task GetTopUpOptions_ShouldReturnMappedOptions()
        {
            // Arrange
            var options = new List<TopUpOption>
            {
                new TopUpOption { Id = 1, Amount = 50 },
                new TopUpOption { Id = 2, Amount = 100 }
            };
            var mappedOptions = new List<TopUpOptionDto>
            {
                new TopUpOptionDto { Id = 1, Amount = 50 },
                new TopUpOptionDto { Id = 2, Amount = 100 }
            };

            _mockRepository.Setup(r => r.GetTopUpOptions()).ReturnsAsync(options);
            _mockMapper.Setup(m => m.Map<IEnumerable<TopUpOptionDto>>(options)).Returns(mappedOptions);

            // Act
            var result = await _service.GetTopUpOptions();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockMapper.Verify(m => m.Map<IEnumerable<TopUpOptionDto>>(options), Times.Once);
        }

        [Fact]
        public async Task TopUpBeneficiary_ValidInput_ShouldReturnTrue()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 50m;

            var beneficiary = new Beneficiary
            {
                Id = beneficiaryId,
                MonthlyTopUpLimit = 300m,
                BeneficiaryTopUp = new List<BeneficiaryTopUpDetails>()
            };

            var user = new User
            {
                Id = userId,
                TotalTopUpLimit = 500m,
                Beneficiaries = new List<Beneficiary> { beneficiary }
            };

            // Setup mock repository methods
            _mockRepository.Setup(r => r.GetUser(userId)).ReturnsAsync(user); // This line was missing
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(100m);
            _mockRepository.Setup(r => r.ValidateUserBalance(100m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.DoPayment(userId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UpdateTransaction(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.CheckUserMonthlyLimit(userId)).Returns(0m);
            _mockRepository.Setup(r => r.UserTopUpLimitPerMonth(beneficiaryId, amount, 0m)).Returns(true);
            _mockRepository.Setup(r => r.ValidatePlan(amount)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.TopUpBeneficiary(userId, beneficiaryId, amount);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task TopUpBeneficiary_UserNotFound_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 50m;

            _mockRepository.Setup(r => r.GetUser(userId)).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task TopUpBeneficiary_BeneficiaryNotFound_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 50m;

            var user = new User
            {
                Id = userId,
                TotalTopUpLimit = 500m,
                Beneficiaries = new List<Beneficiary>()
            };

            _mockRepository.Setup(r => r.GetUser(userId)).ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
            Assert.Equal("Beneficiary not found.", exception.Message);
        }

        [Fact]
        public async Task TopUpBeneficiary_BeneficiaryMonthlyLimitExceeded_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 100m;

            var beneficiary = new Beneficiary
            {
                Id = beneficiaryId,
                MonthlyTopUpLimit = 300m,
                BeneficiaryTopUp = new List<BeneficiaryTopUpDetails>
                {
                    new BeneficiaryTopUpDetails { Amount = 250m, MonthWise = DateTime.Now.Month, YearWise = DateTime.Now.Year }
                }
            };

            var user = new User
            {
                Id = userId,
                TotalTopUpLimit = 500m,
                Beneficiaries = new List<Beneficiary> { beneficiary }
            };

            _mockRepository.Setup(r => r.GetUser(userId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(200m);
            _mockRepository.Setup(r => r.ValidateUserBalance(200m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.DoPayment(userId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UpdateTransaction(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UserTopUpLimitPerMonth(beneficiaryId, amount, 250m)).Returns(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
            Assert.Equal("User top-up Limit exceed for the beneficiary this month...Please wait until next month", exception.Message);
        }

        [Fact]
        public async Task TopUpBeneficiary_UserTotalLimitExceeded_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 200m;

            var beneficiary = new Beneficiary
            {
                Id = beneficiaryId,
                MonthlyTopUpLimit = 300m,
                BeneficiaryTopUp = new List<BeneficiaryTopUpDetails>()
            };

            var user = new User
            {
                Id = userId,
                TotalTopUpLimit = 500m,
                Beneficiaries = new List<Beneficiary> { beneficiary }
            };

            _mockRepository.Setup(r => r.GetUser(userId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(300m);
            _mockRepository.Setup(r => r.ValidateUserBalance(300m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.CheckUserMonthlyLimit(userId)).Returns(500m);
            _mockRepository.Setup(r => r.UserTopUpLimitPerMonth(beneficiaryId, amount, 0m)).Returns(true);
            _mockRepository.Setup(r => r.ValidatePlan(amount)).Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
            Assert.Equal("User Monthly top-up limit exceeded for all beneficiaries.", exception.Message);
        }

        [Fact]
        public async Task TopUpBeneficiary_InvalidTopUpAmount_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = -50m; // Invalid amount

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
            Assert.Equal("Invalid top-up amount.", exception.Message);
        }

        [Fact]
        public async Task TopUpBeneficiary_InvalidTopUpPlan_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 85m;

            var beneficiary = new Beneficiary
            {
                Id = beneficiaryId,
                MonthlyTopUpLimit = 300m,
                BeneficiaryTopUp = new List<BeneficiaryTopUpDetails>()
            };

            var user = new User
            {
                Id = userId,
                TotalTopUpLimit = 500m,
                Beneficiaries = new List<Beneficiary> { beneficiary }
            };

            _mockRepository.Setup(r => r.GetUser(userId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(200m);
            _mockRepository.Setup(r => r.ValidateUserBalance(200m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.ValidatePlan(amount))
                .ThrowsAsync(new Exception("TopUp Plan is Invalid..."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
            Assert.Equal("TopUp Plan is Invalid...", exception.Message);
        }

        [Fact]
        public async Task TopUpBeneficiary_SuccessfulPaymentAndTransactionUpdate_ShouldCallMethods()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 50m;

            var beneficiary = new Beneficiary
            {
                Id = beneficiaryId,
                MonthlyTopUpLimit = 300m,
                BeneficiaryTopUp = new List<BeneficiaryTopUpDetails>()
            };

            var user = new User
            {
                Id = userId,
                TotalTopUpLimit = 500m,
                Beneficiaries = new List<Beneficiary> { beneficiary }
            };

            _mockRepository.Setup(r => r.GetUser(userId)).ReturnsAsync(user);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(100m);
            _mockRepository.Setup(r => r.ValidateUserBalance(100m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.DoPayment(userId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UpdateTransaction(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.CheckUserMonthlyLimit(userId)).Returns(0m);
            _mockRepository.Setup(r => r.UserTopUpLimitPerMonth(beneficiaryId, amount, 0m)).Returns(true);
            _mockRepository.Setup(r => r.ValidatePlan(amount)).Returns(Task.CompletedTask);

            // Act
            await _service.TopUpBeneficiary(userId, beneficiaryId, amount);

            // Assert
            _mockRepository.Verify(r => r.DoPayment(userId, amount), Times.Once);
            _mockRepository.Verify(r => r.UpdateTransaction(userId, beneficiaryId, amount), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}