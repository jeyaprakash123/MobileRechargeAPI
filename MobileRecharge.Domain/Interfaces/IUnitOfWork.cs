using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelecomProviderAPI.Core.IRepository;

namespace MobileRecharge.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IBeneficiaryRepository BeneficiaryRepository { get; }
        IMobileRechargeRepository MobileRechargeRepository { get; }

        Task<int> CompleteAsync(); // Method to save changes
    }
}
