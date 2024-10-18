using Xunit;
using Moq;
using AutoMapper;
using BalanceApi.Models;
using BalanceApi.Services;
using BalanceApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using BalanceApi.DataMapper;

namespace Payment.Api.UnitTests
{
    public class BalanceServiceTests
    {
        private readonly Mock<BalanceDbContext> _mockContext;
        private readonly IMapper _mapper;
        private readonly BalanceService _balanceService;

        public BalanceServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Balance, BalanceDto>();
                cfg.CreateMap<BalanceDto, Balance>();
            });

            _mapper = config.CreateMapper();
            _mockContext = new Mock<BalanceDbContext>(new DbContextOptions<BalanceDbContext>());
            _balanceService = new BalanceService(_mockContext.Object, _mapper);
        }

        // Helper method to create a mock DbSet
        private static Mock<DbSet<T>> CreateMockDbSet<T>(IList<T> list) where T : class
        {
            var queryable = list.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            mockSet.Setup(m => m.AddAsync(It.IsAny<T>(), default)).Callback<T, CancellationToken>((s, _) => list.Add(s));

            return mockSet;
        }

        [Fact]
        public async Task GetUser_ValidId_ShouldReturnBalanceDtos()
        {
            // Arrange
            int userId = 1;
            var balances = new List<Balance>
        {
            new Balance { Id = 1, UserId = userId, BalanceAmount = 100m },
            new Balance { Id = 2, UserId = userId, BalanceAmount = 200m }
        };

            var mockSet = CreateMockDbSet(balances);
            _mockContext.Setup(c => c.Balances).Returns(mockSet.Object);

            // Act
            var result = await _balanceService.GetUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateUser_ValidBalance_ShouldReturnBalanceDto()
        {
            // Arrange
            var balance = new Balance { UserId = 1, BalanceAmount = 100m };
            var balances = new List<Balance>();

            var mockSet = CreateMockDbSet(balances);
            _mockContext.Setup(c => c.Balances).Returns(mockSet.Object);

            // Act
            var result = await _balanceService.CreateUserAsync(balance);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(balance.BalanceAmount, result.BalanceAmount);
        }

        [Fact]
        public async Task UpdateBalance_ValidId_ShouldReturnTrue()
        {
            // Arrange
            int userId = 1;
            var balance = new Balance { Id = userId, UserId = userId, BalanceAmount = 200m };
            var balances = new List<Balance> { balance };

            var mockSet = CreateMockDbSet(balances);
            _mockContext.Setup(c => c.Balances).Returns(mockSet.Object);

            // Act
            var result = await _balanceService.UpdateBalanceAsync(userId, 50m);

            // Assert
            Assert.True(result);
            Assert.Equal(150m, balances.First().BalanceAmount);
        }

        [Fact]
        public async Task UpdateBalance_InvalidId_ShouldReturnFalse()
        {
            // Arrange
            int userId = 1;
            var balances = new List<Balance>();

            var mockSet = CreateMockDbSet(balances);
            _mockContext.Setup(c => c.Balances).Returns(mockSet.Object);

            // Act
            var result = await _balanceService.UpdateBalanceAsync(userId, 50m);

            // Assert
            Assert.False(result);
        }
    }
}