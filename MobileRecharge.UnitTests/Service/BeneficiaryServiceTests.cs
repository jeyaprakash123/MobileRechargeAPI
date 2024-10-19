using AutoMapper;
using MobileRecharge.Domain.UnitOfWork;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelecomProviderAPI.Core.IRepository;
using TopUpAPI.DataMapper;
using TopUpAPI.Models;
using TopUpAPI.Services;
using Xunit;

namespace MobileRecharge.UnitTests.Service
{
    public class BeneficiaryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IBeneficiaryRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper; // Add mock for IMapper
        private readonly BeneficiaryService _service;

        public BeneficiaryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IBeneficiaryRepository>();
            _mockMapper = new Mock<IMapper>(); // Initialize the mapper mock

            _mockUnitOfWork.Setup(uow => uow.BeneficiaryRepository).Returns(_mockRepository.Object);

            // Pass all necessary dependencies to the service constructor
            _service = new BeneficiaryService(_mockUnitOfWork.Object, null, _mockMapper.Object);
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
            var beneficiaryDtos = new List<BeneficiaryDto>
            {
                new BeneficiaryDto { Id = 1, Nickname = "John Doe" }
            };

            _mockRepository.Setup(repo => repo.GetBeneficiaries(userId)).ReturnsAsync(beneficiaries);
            _mockMapper.Setup(m => m.Map<IEnumerable<BeneficiaryDto>>(beneficiaries)).Returns(beneficiaryDtos);

            // Act
            var result = await _service.GetBeneficiaries(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(beneficiaryDtos.Count, result.Count());
        }

        [Fact]
        public async Task AddBeneficiary_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var userId = 1;
            var nickname = "John Doe";
            _mockRepository.Setup(repo => repo.AddBeneficiary(userId, nickname)).ReturnsAsync(true);

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
            _mockRepository.Setup(repo => repo.DeleteBeneficiary(beneficiaryId)).ReturnsAsync(true);

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
            _mockRepository.Setup(repo => repo.GetBeneficiaries(userId)).ReturnsAsync(new List<Beneficiary>());

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
            _mockRepository.Setup(repo => repo.AddBeneficiary(userId, nickname)).ReturnsAsync(false);

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
            _mockRepository.Setup(repo => repo.DeleteBeneficiary(beneficiaryId)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteBeneficiary(beneficiaryId);

            // Assert
            Assert.False(result);
        }
    }
}
