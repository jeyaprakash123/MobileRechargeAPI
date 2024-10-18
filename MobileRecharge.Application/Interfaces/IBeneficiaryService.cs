using TopUpAPI.Models;
using TopUpAPI.DataMapper;

namespace TelecomProviderAPI.Application.Interfaces
{
    public interface IBeneficiaryService
    {
        Task<IEnumerable<BeneficiaryDto>> GetBeneficiaries(int userId);
        Task<bool> AddBeneficiary(int userId, string nickname);
        Task<bool> DeleteBeneficiary(int beneficiaryId);
    }
}
