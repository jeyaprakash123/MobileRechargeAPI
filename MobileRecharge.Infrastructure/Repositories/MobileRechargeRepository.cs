using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MobileRecharge.Domain.Configuration;
using Newtonsoft.Json;
using System.Text;
using TelecomProviderAPI.Core.IRepository;
using TopUpAPI.DataAccess;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Infrastructure.Repositories
{
    public class MobileRechargeRepository:IMobileRechargeRepository
    {
        private readonly TopUpDbContext _context;
        private readonly Appsettings _appSettings;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly decimal charge;
        private readonly IMapper _mapper;

        public MobileRechargeRepository(TopUpDbContext context, IHttpClientFactory httpClientFactory, IOptions<Appsettings> appSettings, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient("PaymentApi");
            _config = config;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TopUpOption>> GetTopUpOptions()
        {
            return await _context.TopUpOptions.ToListAsync();
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _context.Users.AsNoTracking().Include(u => u.Beneficiaries).ThenInclude(b => b.BeneficiaryTopUp)
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task ValidateUserBalance(decimal balance,decimal amount)
        {
            if (balance < amount)
                throw new Exception("Insufficient balance.");
        }
        public async Task ValidatePlan(decimal amount)
        {
            bool isAvail = _context.TopUpOptions.AsNoTracking().Any(t => t.Amount == amount);
            if (!isAvail) throw new Exception("TopUp Plan is Invalid...");
        }
        public async Task UpdateTransaction(int userId,int beneficiaryId,decimal amount)
        {
            var _user = await _context.Users.FindAsync(userId);
            if (_user == null)
            {
                throw new Exception("Failed to update user balance...");
            }
            _user.AvailableBalance = await GetUserBalance(userId);
            var beneficiaryTopUp = new BeneficiaryTopUpDetails
            {
                UserId = userId,
                BeneficiaryId = beneficiaryId,
                Amount = amount,
                Charge = _appSettings.ChargeFee,
                MonthWise = DateTime.Now.Month,
                YearWise = DateTime.Now.Year
            };

            _context.BeneficiaryTopUpDetails.Add(beneficiaryTopUp);
        }
        public async Task DoPayment(int userId,decimal amount)
        {
            decimal TotalAmount = amount + _appSettings.ChargeFee;

            var payload = new
            {
                totalAmount = TotalAmount
            };

            // Serialize the payload to JSON
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var deductContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var deductResponse = await _httpClient.PutAsync($"make-payment?userid={userId}", deductContent);

            if (!deductResponse.IsSuccessStatusCode)
            {
                throw new Exception("Failed to deduct balance from balance service.");
            }
        }
        public async Task<decimal> GetUserBalance(int userId)
        {
            var response = await _httpClient.GetAsync($"get-user-balance?userId={userId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to get balance from balance service.");
            }
            var balanceContent = await response.Content.ReadAsStringAsync();
            var balance=decimal.Parse(balanceContent);
            return balance;
        }
        public bool UserTopUpLimitPerMonth(int beneficiaryId, decimal amount, decimal totalTopUpsThisMonth)
        {
            var beneficiary = _context.Beneficiaries.AsNoTracking().FirstOrDefaultAsync(u => u.Id == beneficiaryId);
            if (totalTopUpsThisMonth + amount >= beneficiary.Result.MonthlyTopUpLimit)
            {
                return false;
            }
            return true; 
        }
        public decimal CheckUserMonthlyLimit(int userId)
        {
            return _context.BeneficiaryTopUpDetails
                                .Where(x => x.UserId == userId
                                   && x.MonthWise == DateTime.Now.Month
                                   && x.YearWise == DateTime.Now.Year)
                                .Sum(x => x.Amount);
        }
    }
}
