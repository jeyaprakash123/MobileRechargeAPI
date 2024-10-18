using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Core.IRepository
{
    public interface IBeneficiaryRepository
    {
        Task<IEnumerable<Beneficiary>> GetBeneficiaries(int userId);
        Task<bool> AddBeneficiary(int userId, string nickname);
        Task<bool> DeleteBeneficiary(int beneficiaryId);
    }
}
