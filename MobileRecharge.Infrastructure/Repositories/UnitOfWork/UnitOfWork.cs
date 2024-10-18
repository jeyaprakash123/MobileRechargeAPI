using TelecomProviderAPI.Core.IRepository;
using TopUpAPI.DataAccess;
using MobileRecharge.Domain.UnitOfWork;

namespace MobileRecharge.Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TopUpDbContext _context;

        public UnitOfWork(IUserRepository userRepository,
                          IBeneficiaryRepository beneficiaryRepository,
                          IMobileRechargeRepository mobileRechargeRepository,
                          TopUpDbContext context)
        {
            UserRepository = userRepository;
            BeneficiaryRepository = beneficiaryRepository;
            MobileRechargeRepository = mobileRechargeRepository;
            _context = context;
        }
        public IUserRepository UserRepository { get; }
        public IBeneficiaryRepository BeneficiaryRepository { get; }
        public IMobileRechargeRepository MobileRechargeRepository { get; }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
