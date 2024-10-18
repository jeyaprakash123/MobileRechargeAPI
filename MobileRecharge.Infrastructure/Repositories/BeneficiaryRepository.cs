using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MobileRecharge.Domain.Configuration;
using TelecomProviderAPI.Core.IRepository;
using TopUpAPI.DataAccess;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Infrastructure.Repositories
{
    public class BeneficiaryRepository:IBeneficiaryRepository
    {
        private readonly TopUpDbContext _context;
        private readonly IConfiguration _config;
        private readonly Appsettings _appSettings;
        private readonly IMapper _mapper;

        public BeneficiaryRepository(TopUpDbContext context, IConfiguration config, IMapper mapper, IOptions<Appsettings> appSettings)
        {
            _context = context;
            _config = config;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Beneficiary>> GetBeneficiaries(int userId)
        {
            return await _context.Beneficiaries
                .Where(b => b.UserId == userId).ToListAsync();
        }

        public async Task<bool> AddBeneficiary(int userId, string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname) || nickname.Length > 20)
                throw new ArgumentException("Nickname must be 20 characters or less.");
               

            var user = await _context.Users.Include(u => u.Beneficiaries).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found.");

            if (user.Beneficiaries.Count >= _appSettings.MaxBeneficiaryPerUser)
                throw new Exception("Maximum of 5 beneficiaries allowed.");

            var beneficiary = new Beneficiary
            {
                UserId = userId,
                Nickname = nickname,
                MonthlyTopUpLimit = user.IsVerified ? _appSettings.MaximumRechargePerMonthForVerifiedUser : _appSettings.MaximumRechargePerMonthForNotverifiedUser
            };

            _context.Beneficiaries.Add(beneficiary);
            return true;
        }
        public async Task<bool> DeleteBeneficiary(int beneficiaryId)
        {
            var user = await _context.Beneficiaries.FindAsync(beneficiaryId);
            if (user == null)
            {
                return false;
            }
            _context.Beneficiaries.Remove(user);
            return true;
        }
    }
}
