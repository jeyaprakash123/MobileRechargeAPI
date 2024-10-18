using BalanceApi.Models;
using Microsoft.AspNetCore.Mvc;
using BalanceApi.DataMapper;

namespace BalanceApi.Services
{
    public interface IBalanceService
    {
        Task<IEnumerable<BalanceDto>> GetUser(int id);
        Task<BalanceDto> CreateUserAsync(Balance balance);
        Task<bool> UpdateBalanceAsync(int id, decimal amount);
    }
}
