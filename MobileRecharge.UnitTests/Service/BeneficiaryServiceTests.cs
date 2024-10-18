using MobileRecharge.Domain.UnitOfWork;
using Moq;
using TelecomProviderAPI.Core.IRepository;
using TopUpAPI.Models;
using TopUpAPI.Services;
using Xunit;


namespace MobileRecharge.UnitTests.Service
{
    public class BeneficiaryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IBeneficiaryRepository> _mockRepository;
        private readonly BeneficiaryService _service;

        public BeneficiaryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IBeneficiaryRepository>();
            _mockUnitOfWork.Setup(uow => uow.BeneficiaryRepository).Returns(_mockRepository.Object);

            _service = new BeneficiaryService(_mockUnitOfWork.Object, null, null);
        }

        [Fact]
        public async Task GetBeneficiaries_ReturnsMappedBeneficiaries_WhenSuccessful()
        {
            // Arrange
            var userId = 1;
            var beneficiaries = new List<Beneficiary>
        {
            new Beneficiary { Id = 1, Nickname = "John Doe" }
        };

            _mockRepository.Setup(repo => repo.GetBeneficiaries(userId))
                           .ReturnsAsync(beneficiaries);

            // Act
            var result = await _service.GetBeneficiaries(userId);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task AddBeneficiary_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var userId = 1;
            var nickname = "John Doe";
            _mockRepository.Setup(repo => repo.AddBeneficiary(userId, nickname))
                           .ReturnsAsync(true);

            // Act
            var result = await _service.AddBeneficiary(userId, nickname);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteBeneficiary_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var beneficiaryId = 1;
            _mockRepository.Setup(repo => repo.DeleteBeneficiary(beneficiaryId))
                           .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteBeneficiary(beneficiaryId);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public async Task GetBeneficiaries_ReturnsEmptyList_WhenNoBeneficiariesFound()
        {
            // Arrange
            var userId = 1;
            _mockRepository.Setup(repo => repo.GetBeneficiaries(userId))
                           .ReturnsAsync(new List<Beneficiary>());

            // Act
            var result = await _service.GetBeneficiaries(userId);

            // Assert
            Assert.Empty(result);
        }
        [Fact]
        public async Task AddBeneficiary_ReturnsFalse_WhenRepositoryFails()
        {
            // Arrange
            var userId = 1;
            var nickname = "John Doe";
            _mockRepository.Setup(repo => repo.AddBeneficiary(userId, nickname))
                           .ReturnsAsync(false);

            // Act
            var result = await _service.AddBeneficiary(userId, nickname);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task DeleteBeneficiary_ReturnsFalse_WhenBeneficiaryDoesNotExist()
        {
            // Arrange
            var beneficiaryId = 1;
            _mockRepository.Setup(repo => repo.DeleteBeneficiary(beneficiaryId))
                           .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteBeneficiary(beneficiaryId);

            // Assert
            Assert.False(result);
        }


    }
}
