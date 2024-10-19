using TopUpAPI.DataMapper;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Application.Interfaces
{
    public interface IMobileRechargeService
    {
        Task<IEnumerable<TopUpOptionDto>> GetTopUpOptions();
        Task<bool> TopUpBeneficiary(int userId, int beneficiaryId, decimal amount);
    }
}
