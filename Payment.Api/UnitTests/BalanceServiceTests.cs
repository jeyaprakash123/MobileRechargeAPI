using BalanceApi.DataAccess;
using BalanceApi.Models;
using BalanceApi.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BalanceApi.DataMapper;

namespace Payment.Api.UnitTests
{
    public class BalanceServiceTests
    {
        private readonly BalanceDbContext _context;
        private readonly IMapper _mapper;
        private readonly BalanceService _service;

        public BalanceServiceTests()
        {
            var options = new DbContextOptionsBuilder<BalanceDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new BalanceDbContext(options);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Balance, BalanceDto>();
                cfg.CreateMap<BalanceDto, Balance>();
            });
            _mapper = config.CreateMapper();
            _service = new BalanceService(_context, _mapper);
        }

        [Fact]
        public async Task GetUser_ReturnsBalances_ForExistingUser()
        {
            // Arrange
            var userId = 1;
            _context.Balances.Add(new Balance { Id = 1, UserId = userId, BalanceAmount = 100 });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetUser(userId);

            // Assert
            Assert.Single(result);
            Assert.Equal(100, result.First().BalanceAmount);
        }

        [Fact]
        public async Task CreateUserAsync_AddsNewBalance()
        {
            // Arrange
            var newBalance = new Balance { UserId = 2, BalanceAmount = 200 };

            // Act
            var result = await _service.CreateUserAsync(newBalance);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newBalance.UserId, result.UserId);
            Assert.Equal(newBalance.BalanceAmount, result.BalanceAmount);
        }

        [Fact]
        public async Task UpdateBalanceAsync_UpdatesBalance_WhenSufficientFunds()
        {
            // Arrange
            var balance = new Balance { Id = 1, UserId = 1, BalanceAmount = 100 };
            _context.Balances.Add(balance);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.UpdateBalanceAsync(1, 50);

            // Assert
            Assert.True(result);
            var updatedBalance = await _context.Balances.FindAsync(1);
            Assert.Equal(50, updatedBalance.BalanceAmount);
        }

        [Fact]
        public async Task UpdateBalanceAsync_ThrowsException_WhenInsufficientFunds()
        {
            // Arrange
            var balance = new Balance { Id = 1, UserId = 1, BalanceAmount = 50 };
            _context.Balances.Add(balance);
            await _context.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateBalanceAsync(1, 100));
            Assert.Equal("Insufficient balance.", exception.Message);
        }

        [Fact]
        public async Task UserExists_ReturnsTrue_ForExistingUser()
        {
            // Arrange
            var balance = new Balance { Id = 1, UserId = 1, BalanceAmount = 100 };
            _context.Balances.Add(balance);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.UserExists(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserExists_ReturnsFalse_ForNonExistingUser()
        {
            // Act
            var result = await _service.UserExists(99);

            // Assert
            Assert.False(result);
        }
    }
}
