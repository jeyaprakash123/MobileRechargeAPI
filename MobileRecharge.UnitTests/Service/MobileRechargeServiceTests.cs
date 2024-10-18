using MobileRecharge.Domain.UnitOfWork;
using Moq;
using TelecomProviderAPI.Core.IRepository;
using TelecomProviderAPI.Services;
using Xunit;


namespace MobileRecharge.UnitTests.Service
{
    public class MobileRechargeServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMobileRechargeRepository> _mockRepository;
        private readonly MobileRechargeService _service;

        public MobileRechargeServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IMobileRechargeRepository>();
            _mockUnitOfWork.Setup(u => u.MobileRechargeRepository).Returns(_mockRepository.Object);
            _service = new MobileRechargeService(_mockUnitOfWork.Object, null, null, null);
        }

        [Fact]
        public async Task TopUpBeneficiary_ValidInput_ShouldReturnTrue()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 50m;

            _mockRepository.Setup(r => r.TopUpBeneficiary(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(100m);
            _mockRepository.Setup(r => r.ValidateUserBalance(100m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.DoPayment(userId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UpdateTransaction(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1); 


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

            _mockRepository.Setup(r => r.TopUpBeneficiary(userId, beneficiaryId, amount)).ThrowsAsync(new Exception("User not found."));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
        }

        [Fact]
        public async Task TopUpBeneficiary_InsufficientBalance_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 150m;

            _mockRepository.Setup(r => r.TopUpBeneficiary(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(100m);
            _mockRepository.Setup(r => r.ValidateUserBalance(100m, amount)).ThrowsAsync(new Exception("Insufficient balance."));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
        }
        [Fact]
        public async Task TopUpBeneficiary_BeneficiaryMonthlyLimitExceeded_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 100m;

            _mockRepository.Setup(r => r.TopUpBeneficiary(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(200m);
            _mockRepository.Setup(r => r.ValidateUserBalance(200m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.DoPayment(userId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UpdateTransaction(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UserTopUpLimitPerMonth(beneficiaryId, amount, It.IsAny<decimal>())).Returns(false); // Exceeds limit

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
        }

        [Fact]
        public async Task TopUpBeneficiary_UserTotalLimitExceeded_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 200m;

            _mockRepository.Setup(r => r.TopUpBeneficiary(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(300m);
            _mockRepository.Setup(r => r.ValidateUserBalance(300m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.DoPayment(userId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UpdateTransaction(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.CheckUserMonthlyLimit(userId)).Returns(500m); // Simulating user's total top-ups
            _mockRepository.Setup(r => r.UserTopUpLimitPerMonth(beneficiaryId, amount, It.IsAny<decimal>())).Returns(true);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
        }

        [Fact]
        public async Task TopUpBeneficiary_InvalidTopUpAmount_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = -50m; // Invalid amount

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
        }

        [Fact]
        public async Task TopUpBeneficiary_InvalidTopUpPlan_ShouldThrowException()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 85m;

            _mockRepository.Setup(r => r.TopUpBeneficiary(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(200m);
            _mockRepository.Setup(r => r.ValidateUserBalance(200m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.DoPayment(userId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UpdateTransaction(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.ValidatePlan(amount)).ThrowsAsync(new Exception("TopUp Plan is Invalid...")); // Invalid plan

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.TopUpBeneficiary(userId, beneficiaryId, amount));
        }

        [Fact]
        public async Task TopUpBeneficiary_SuccessfulPaymentAndTransactionUpdate_ShouldCallMethods()
        {
            // Arrange
            int userId = 1;
            int beneficiaryId = 1;
            decimal amount = 50m;

            _mockRepository.Setup(r => r.TopUpBeneficiary(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.GetUserBalance(userId)).ReturnsAsync(100m);
            _mockRepository.Setup(r => r.ValidateUserBalance(100m, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.DoPayment(userId, amount)).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.UpdateTransaction(userId, beneficiaryId, amount)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);


            // Act
            await _service.TopUpBeneficiary(userId, beneficiaryId, amount);

            // Assert
            _mockRepository.Verify(r => r.DoPayment(userId, amount), Times.Once);
            _mockRepository.Verify(r => r.UpdateTransaction(userId, beneficiaryId, amount), Times.Once);
        }
    }
}
