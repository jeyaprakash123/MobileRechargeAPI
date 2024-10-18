using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Core.IRepository
{
    public interface IMobileRechargeRepository
    {
        Task<IEnumerable<TopUpOption>> GetTopUpOptions();
        Task TopUpBeneficiary(int userId, int beneficiaryId, decimal amount);
        Task<decimal> GetUserBalance(int userId);
        Task DoPayment(int userId, decimal amount);
        Task UpdateTransaction(int userId, int beneficiaryId, decimal amount);
        Task ValidatePlan(decimal amount);
        bool UserTopUpLimitPerMonth(int beneficiaryId, decimal amount, decimal totalTopUpsThisMonth);
        Task ValidateUserBalance(decimal balance, decimal amount);
        decimal CheckUserMonthlyLimit(int userId);
    }
}
